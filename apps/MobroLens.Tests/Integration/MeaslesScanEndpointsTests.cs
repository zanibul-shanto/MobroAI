using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MobroLens.Tests.Integration;

public class MeaslesScanEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public MeaslesScanEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private async Task<(User user, string token, Child child)> SetupUserAndChildAsync()
    {
        var email = $"scan_{Guid.NewGuid()}@test.com";
        var registerRequest = new RegisterRequest(email, "Password123!", "Test Parent", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var loginRequest = new LoginRequest(email, "Password123!");
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        using var context = GetDbContext();
        var user = await context.Users.FirstAsync(u => u.Email == email);

        // Create child
        var child = new Child
        {
            Id = Guid.NewGuid(),
            ParentId = user.Id,
            FullName = "Test Child",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        context.Children.Add(child);
        await context.SaveChangesAsync();

        return (user, authResponse!.AccessToken, child);
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/scans");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var scan = new MeaslesScan
        {
            ChildId = child.Id,
            Status = ScanStatus.Pending
        };

        var response = await _client.PostAsJsonAsync("/scans", scan);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdScan = await response.Content.ReadFromJsonAsync<MeaslesScan>();
        Assert.NotNull(createdScan);
        Assert.NotEqual(Guid.Empty, createdScan.Id);
        Assert.Equal(user.Id, createdScan.UploadedById);
    }

    [Fact]
    public async Task GetById_WithExistingScan_ReturnsScan()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create scan
        var scan = new MeaslesScan
        {
            ChildId = child.Id,
            Status = ScanStatus.Pending
        };
        var createResponse = await _client.PostAsJsonAsync("/scans", scan);
        var createdScan = await createResponse.Content.ReadFromJsonAsync<MeaslesScan>();

        // Get by ID
        var response = await _client.GetAsync($"/scans/{createdScan!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<MeaslesScan>();
        Assert.NotNull(result);
        Assert.Equal(createdScan.Id, result.Id);
    }

    [Fact]
    public async Task GetByChild_ReturnsScansForChild()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create multiple scans
        for (int i = 0; i < 3; i++)
        {
            var scan = new MeaslesScan
            {
                ChildId = child.Id,
                Status = ScanStatus.Pending
            };
            await _client.PostAsJsonAsync("/scans", scan);
        }

        var response = await _client.GetAsync($"/scans/child/{child.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var scans = await response.Content.ReadFromJsonAsync<List<MeaslesScan>>();
        Assert.NotNull(scans);
        Assert.Equal(3, scans.Count);
        Assert.All(scans, s => Assert.Equal(child.Id, s.ChildId));
    }

    [Fact]
    public async Task UpdateStatus_WithValidStatus_ReturnsNoContent()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create scan
        var scan = new MeaslesScan
        {
            ChildId = child.Id,
            Status = ScanStatus.Pending
        };
        var createResponse = await _client.PostAsJsonAsync("/scans", scan);
        var createdScan = await createResponse.Content.ReadFromJsonAsync<MeaslesScan>();

        // Update status
        var response = await _client.PutAsJsonAsync($"/scans/{createdScan!.Id}/status", ScanStatus.Cleared);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify
        using var context = GetDbContext();
        var dbScan = await context.MeaslesScans.FindAsync(createdScan.Id);
        Assert.Equal(ScanStatus.Cleared, dbScan!.Status);
        Assert.True(dbScan.UpdatedAt > dbScan.CreatedAt);
    }

    [Fact]
    public async Task GetByUser_ReturnsScansForUser()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create scans
        for (int i = 0; i < 3; i++)
        {
            var scan = new MeaslesScan
            {
                ChildId = child.Id,
                Status = ScanStatus.Pending
            };
            await _client.PostAsJsonAsync("/scans", scan);
        }

        var response = await _client.GetAsync($"/scans/user/{user.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var scans = await response.Content.ReadFromJsonAsync<List<MeaslesScan>>();
        Assert.NotNull(scans);
        Assert.True(scans.Count >= 3);
        Assert.All(scans, s => Assert.Equal(user.Id, s.UploadedById));
    }

    [Fact]
    public async Task GetByChild_ReturnsInDescendingOrder()
    {
        var (user, token, child) = await SetupUserAndChildAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create scans with delay
        for (int i = 0; i < 3; i++)
        {
            var scan = new MeaslesScan
            {
                ChildId = child.Id,
                Status = ScanStatus.Pending
            };
            await _client.PostAsJsonAsync("/scans", scan);
            await Task.Delay(10);
        }

        var response = await _client.GetAsync($"/scans/child/{child.Id}");
        var scans = await response.Content.ReadFromJsonAsync<List<MeaslesScan>>();

        Assert.NotNull(scans);
        if (scans.Count >= 2)
        {
            for (int i = 0; i < scans.Count - 1; i++)
            {
                Assert.True(scans[i].CreatedAt >= scans[i + 1].CreatedAt);
            }
        }
    }
}

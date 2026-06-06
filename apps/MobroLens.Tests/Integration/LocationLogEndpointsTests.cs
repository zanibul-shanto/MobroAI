using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MobroLens.Tests.Integration;

public class LocationLogEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public LocationLogEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private async Task<(User user, string token)> CreateUserAndGetTokenAsync()
    {
        var email = $"location_{Guid.NewGuid()}@test.com";
        var registerRequest = new RegisterRequest(email, "Password123!", "Test User", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var loginRequest = new LoginRequest(email, "Password123!");
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        using var context = GetDbContext();
        var user = await context.Users.FirstAsync(u => u.Email == email);

        return (user, authResponse!.AccessToken);
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/locations");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var location = new LocationLog
        {
            Latitude = 23.8103m,
            Longitude = 90.4125m,
            WithChild = true
        };

        var response = await _client.PostAsJsonAsync("/locations", location);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdLocation = await response.Content.ReadFromJsonAsync<LocationLog>();
        Assert.NotNull(createdLocation);
        Assert.NotEqual(Guid.Empty, createdLocation.Id);
        Assert.Equal(23.8103m, createdLocation.Latitude);
        Assert.Equal(90.4125m, createdLocation.Longitude);
    }

    [Fact]
    public async Task Create_SetsUserIdFromClaims()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var location = new LocationLog
        {
            Latitude = 23.8103m,
            Longitude = 90.4125m,
            WithChild = false
        };

        var response = await _client.PostAsJsonAsync("/locations", location);
        var createdLocation = await response.Content.ReadFromJsonAsync<LocationLog>();

        Assert.NotNull(createdLocation);
        Assert.Equal(user.Id, createdLocation.UserId);
    }

    [Fact]
    public async Task GetAll_WithAuth_ReturnsLocationsList()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create some locations
        for (int i = 0; i < 3; i++)
        {
            var location = new LocationLog
            {
                Latitude = 23.8103m + i,
                Longitude = 90.4125m + i,
                WithChild = i % 2 == 0
            };
            await _client.PostAsJsonAsync("/locations", location);
        }

        var response = await _client.GetAsync("/locations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var locations = await response.Content.ReadFromJsonAsync<List<LocationLog>>();
        Assert.NotNull(locations);
        Assert.True(locations.Count >= 3);
    }

    [Fact]
    public async Task GetByUserId_WithLocations_ReturnsFilteredList()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create locations
        for (int i = 0; i < 3; i++)
        {
            var location = new LocationLog
            {
                Latitude = 23.8103m + i,
                Longitude = 90.4125m + i,
                WithChild = false
            };
            await _client.PostAsJsonAsync("/locations", location);
        }

        var response = await _client.GetAsync($"/locations/user/{user.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var locations = await response.Content.ReadFromJsonAsync<List<LocationLog>>();
        Assert.NotNull(locations);
        Assert.True(locations.Count >= 3);
        Assert.All(locations, l => Assert.True(l.UserId == user.Id));
    }

    [Fact]
    public async Task GetByUserId_ReturnsInDescendingOrder()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create locations
        for (int i = 0; i < 3; i++)
        {
            await _client.PostAsJsonAsync("/locations", new LocationLog
            {
                Latitude = 23.8103m,
                Longitude = 90.4125m,
                WithChild = false
            });
            await Task.Delay(10); // Small delay to ensure different timestamps
        }

        var response = await _client.GetAsync($"/locations/user/{user.Id}");
        var locations = await response.Content.ReadFromJsonAsync<List<LocationLog>>();

        Assert.NotNull(locations);
        if (locations.Count >= 2)
        {
            // Check descending order
            for (int i = 0; i < locations.Count - 1; i++)
            {
                Assert.True(locations[i].CreatedAt >= locations[i + 1].CreatedAt);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MobroLens.Tests.Integration;

public class ChildEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ChildEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private async Task<(User user, string token)> CreateUserAndGetTokenAsync(
        string? email = null,
        Role role = Role.Parent)
    {
        email ??= $"parent_{Guid.NewGuid():N}@test.com";
        var registerRequest = new RegisterRequest(email, "Password123!", "Test Parent", null, role);
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
        var response = await _client.GetAsync("/children");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Jane Doe",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Female
        };

        var response = await _client.PostAsJsonAsync("/children", child);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdChild = await response.Content.ReadFromJsonAsync<Child>();
        Assert.NotNull(createdChild);
        Assert.NotEqual(Guid.Empty, createdChild.Id);
        Assert.Equal("Jane Doe", createdChild.FullName);
    }

    [Fact]
    public async Task GetById_WithExistingChild_ReturnsChild()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Test Child",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Get by ID
        var response = await _client.GetAsync($"/children/{createdChild!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Child>();
        Assert.NotNull(result);
        Assert.Equal(createdChild.Id, result.Id);
    }

    [Fact]
    public async Task GetById_WithNonexistentChild_ReturnsNotFound()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/children/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByParent_WithChildren_ReturnsList()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create children
        for (int i = 0; i < 3; i++)
        {
            var child = new Child
            {
                ParentId = parent.Id,
                FullName = $"Child {i}",
                DateOfBirth = new DateTime(2020, 1, 15),
                Gender = Gender.Male
            };
            await _client.PostAsJsonAsync("/children", child);
        }

        var response = await _client.GetAsync($"/children/parent/{parent.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var children = await response.Content.ReadFromJsonAsync<List<Child>>();
        Assert.NotNull(children);
        Assert.Equal(3, children.Count);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsNoContent()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Original Name",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Update
        var updatedChild = new Child
        {
            FullName = "Updated Name",
            DateOfBirth = new DateTime(2019, 5, 20),
            Gender = Gender.Female,
            ParentId = parent.Id
        };
        var response = await _client.PutAsJsonAsync($"/children/{createdChild!.Id}", updatedChild);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify
        var getResponse = await _client.GetAsync($"/children/{createdChild.Id}");
        var result = await getResponse.Content.ReadFromJsonAsync<Child>();
        Assert.Equal("Updated Name", result!.FullName);
        Assert.Equal(Gender.Female, result.Gender);
    }

    [Fact]
    public async Task Delete_WithExistingChild_ReturnsNoContent()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "To Delete",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Delete
        var response = await _client.DeleteAsync($"/children/{createdChild!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify
        var getResponse = await _client.GetAsync($"/children/{createdChild.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task VaccineDate_AddVaccineDate_ReturnsCreated()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Vaccine Child",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Add vaccine date
        var vaccineDate = new VaccineDate
        {
            Date = new DateTime(2024, 6, 15),
            Note = "BCG Vaccine"
        };
        var response = await _client.PostAsJsonAsync($"/children/{createdChild!.Id}/vaccines", vaccineDate);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<VaccineDate>();
        Assert.NotNull(result);
        Assert.Equal("BCG Vaccine", result.Note);
    }

    [Fact]
    public async Task VaccineDate_GetVaccineDates_ReturnsList()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Vaccine Child",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Add vaccine dates
        for (int i = 0; i < 3; i++)
        {
            var vaccineDate = new VaccineDate
            {
                Date = new DateTime(2024, 6, 15).AddMonths(i),
                Note = $"Vaccine {i}"
            };
            await _client.PostAsJsonAsync($"/children/{createdChild!.Id}/vaccines", vaccineDate);
        }

        var response = await _client.GetAsync($"/children/{createdChild!.Id}/vaccines");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<List<VaccineDate>>();
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetUpcomingVaccines_ReturnsFutureDates()
    {
        var (parent, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create child
        var child = new Child
        {
            ParentId = parent.Id,
            FullName = "Upcoming Child",
            DateOfBirth = new DateTime(2020, 1, 15),
            Gender = Gender.Male
        };
        var createResponse = await _client.PostAsJsonAsync("/children", child);
        var createdChild = await createResponse.Content.ReadFromJsonAsync<Child>();

        // Add future and past vaccine dates
        var pastDate = new VaccineDate { Date = DateTime.UtcNow.AddDays(-10), Note = "Past" };
        var futureDate = new VaccineDate { Date = DateTime.UtcNow.AddDays(10), Note = "Future" };
        await _client.PostAsJsonAsync($"/children/{createdChild!.Id}/vaccines", pastDate);
        await _client.PostAsJsonAsync($"/children/{createdChild.Id}/vaccines", futureDate);

        var response = await _client.GetAsync($"/children/parent/{parent.Id}/upcoming-vaccines");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

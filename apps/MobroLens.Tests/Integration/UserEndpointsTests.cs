using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MobroLens.Tests.Integration;

public class UserEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UserEndpointsTests(CustomWebApplicationFactory factory)
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
        string password = "Password123!",
        Role role = Role.Parent)
    {
        email ??= $"user_{Guid.NewGuid():N}@test.com";

        // Register user
        var registerRequest = new RegisterRequest(email, password, "Test User", null, role);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        // Login to get token
        var loginRequest = new LoginRequest(email, password);
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        using var context = GetDbContext();
        var user = await context.Users.FirstAsync(u => u.Email == email);

        return (user, authResponse!.AccessToken);
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/users");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WithAuth_ReturnsUsersList()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task GetById_WithExistingUser_ReturnsUser()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/users/{user.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetById_WithNonexistentUser_ReturnsNotFound()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newUser = new User
        {
            Email = "newuser@example.com",
            FullName = "New User",
            PasswordHash = "Password123!",
            Role = Role.Parent
        };

        var response = await _client.PostAsJsonAsync("/users", newUser);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdUser = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);
        Assert.NotEqual(Guid.Empty, createdUser.Id);
    }

    [Fact]
    public async Task Create_HashesPassword()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var password = "PlainPassword123!";
        var newUser = new User
        {
            Email = "hashtest@example.com",
            FullName = "Hash Test",
            PasswordHash = password,
            Role = Role.Parent
        };

        await _client.PostAsJsonAsync("/users", newUser);

        using var context = GetDbContext();
        var dbUser = await context.Users.FirstAsync(u => u.Email == "hashtest@example.com");

        Assert.NotEqual(password, dbUser.PasswordHash);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, dbUser.PasswordHash));
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsNoContent()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedUser = new User
        {
            Email = user.Email,
            FullName = "Updated Name",
            PhoneNumber = "+1234567890",
            Role = user.Role
        };

        var response = await _client.PutAsJsonAsync($"/users/{user.Id}", updatedUser);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify update
        using var context = GetDbContext();
        var dbUser = await context.Users.FindAsync(user.Id);
        Assert.Equal("Updated Name", dbUser!.FullName);
        Assert.Equal("+1234567890", dbUser.PhoneNumber);
    }

    [Fact]
    public async Task Update_WithNonexistentUser_ReturnsNotFound()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedUser = new User { Email = "test@test.com", FullName = "Test" };
        var response = await _client.PutAsJsonAsync($"/users/{Guid.NewGuid()}", updatedUser);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithExistingUser_ReturnsNoContent()
    {
        var (user, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"/users/{user.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify deletion
        using var context = GetDbContext();
        var dbUser = await context.Users.FindAsync(user.Id);
        Assert.Null(dbUser);
    }

    [Fact]
    public async Task Delete_WithNonexistentUser_ReturnsNotFound()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WithMatchingPasswords_ReturnsOk()
    {
        var (user, token) = await CreateUserAndGetTokenAsync(password: "OldPass123!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new ChangePasswordRequest("NewPass123!", "NewPass123!");
        var response = await _client.PostAsJsonAsync("/users/change-password", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify new password works
        var loginRequest = new LoginRequest(user.Email, "NewPass123!");
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WithMismatchedPasswords_ReturnsBadRequest()
    {
        var (_, token) = await CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new ChangePasswordRequest("NewPass123!", "DifferentPass123!");
        var response = await _client.PostAsJsonAsync("/users/change-password", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WithoutAuth_ReturnsUnauthorized()
    {
        var request = new ChangePasswordRequest("NewPass123!", "NewPass123!");
        var response = await _client.PostAsJsonAsync("/users/change-password", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

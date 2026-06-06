using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;

namespace MobroLens.Tests.Integration;

public class AuthEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOk()
    {
        var request = new RegisterRequest(
            "test@example.com",
            "Password123!",
            "Test User",
            null,
            Role.Parent
        );

        var response = await _client.PostAsJsonAsync("/auth/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("created", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        var request = new RegisterRequest(
            "duplicate@example.com",
            "Password123!",
            "Test User",
            null,
            Role.Parent
        );

        // First registration
        await _client.PostAsJsonAsync("/auth/register", request);
        // Second registration with same email
        var response = await _client.PostAsJsonAsync("/auth/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_SavesUserToDatabase()
    {
        var email = $"dbtest_{Guid.NewGuid()}@example.com";
        var request = new RegisterRequest(
            email,
            "Password123!",
            "DB Test User",
            null,
            Role.Parent
        );

        await _client.PostAsJsonAsync("/auth/register", request);

        using var context = GetDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        Assert.NotNull(user);
        Assert.Equal("DB Test User", user.FullName);
        Assert.Equal(Role.Parent, user.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify("Password123!", user.PasswordHash));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAuthResponse()
    {
        // Register a user first
        var email = "login@test.com";
        var password = "Password123!";
        var registerRequest = new RegisterRequest(email, password, "Login Test", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        // Login
        var loginRequest = new LoginRequest(email, password);
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(authResponse);
        Assert.False(string.IsNullOrEmpty(authResponse.AccessToken));
        Assert.NotNull(authResponse.User);
        Assert.Equal(email, authResponse.User.Email);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var loginRequest = new LoginRequest("nonexistent@example.com", "wrongpassword");
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        // Register a user
        var email = "wrongpass@test.com";
        var registerRequest = new RegisterRequest(email, "CorrectPass123!", "Test", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        // Try login with wrong password
        var loginRequest = new LoginRequest(email, "WrongPass123!");
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ForgotPassword_WithExistingUser_ReturnsOk()
    {
        // Register a user
        var email = "forgot@test.com";
        var registerRequest = new RegisterRequest(email, "Pass123!", "Test", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var request = new ForgotPasswordRequest(email);
        var response = await _client.PostAsJsonAsync("/auth/forgot-password", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ForgotPassword_WithNonexistentUser_ReturnsNotFound()
    {
        var request = new ForgotPasswordRequest("nonexistent@example.com");
        var response = await _client.PostAsJsonAsync("/auth/forgot-password", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_WithValidCode_ReturnsOk()
    {
        // Register a user
        var email = "reset@test.com";
        var registerRequest = new RegisterRequest(email, "OldPass123!", "Test", null, Role.Parent);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var request = new ResetPasswordRequest(email, "12345", "NewPass123!");
        var response = await _client.PostAsJsonAsync("/auth/reset-password", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify new password works
        var loginRequest = new LoginRequest(email, "NewPass123!");
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_WithInvalidCode_ReturnsBadRequest()
    {
        var request = new ResetPasswordRequest("test@example.com", "wrongcode", "NewPass123!");
        var response = await _client.PostAsJsonAsync("/auth/reset-password", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

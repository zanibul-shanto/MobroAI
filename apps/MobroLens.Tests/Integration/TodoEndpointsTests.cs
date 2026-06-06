using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Models;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace MobroLens.Tests.Integration;

public class TodoEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public TodoEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    private async Task<string> GetTokenAsync(Role role = Role.Parent)
    {
        var email = $"todo_{Guid.NewGuid()}@test.com";
        var registerRequest = new RegisterRequest(email, "Password123!", "Test User", null, role);
        await _client.PostAsJsonAsync("/auth/register", registerRequest);

        var loginRequest = new LoginRequest(email, "Password123!");
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        return authResponse!.AccessToken;
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/todoitems");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WithAuth_ReturnsTodosList()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/todoitems");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
        Assert.NotNull(todos);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var todo = new Todo { Name = "Test Todo", IsComplete = false };

        var response = await _client.PostAsJsonAsync("/todoitems", todo);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdTodo = await response.Content.ReadFromJsonAsync<Todo>();
        Assert.NotNull(createdTodo);
        Assert.Equal("Test Todo", createdTodo.Name);
        Assert.False(createdTodo.IsComplete);
    }

    [Fact]
    public async Task GetById_WithExistingTodo_ReturnsTodo()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create todo
        var todo = new Todo { Name = "Find Me", IsComplete = false };
        var createResponse = await _client.PostAsJsonAsync("/todoitems", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Get by ID
        var response = await _client.GetAsync($"/todoitems/{createdTodo!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Todo>();
        Assert.NotNull(result);
        Assert.Equal(createdTodo.Id, result.Id);
        Assert.Equal("Find Me", result.Name);
    }

    [Fact]
    public async Task GetById_WithNonexistentTodo_ReturnsNotFound()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/todoitems/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsNoContent()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create todo
        var todo = new Todo { Name = "Original", IsComplete = false };
        var createResponse = await _client.PostAsJsonAsync("/todoitems", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Update
        var updatedTodo = new Todo { Name = "Updated", IsComplete = true };
        var response = await _client.PutAsJsonAsync($"/todoitems/{createdTodo!.Id}", updatedTodo);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify
        var getResponse = await _client.GetAsync($"/todoitems/{createdTodo.Id}");
        var result = await getResponse.Content.ReadFromJsonAsync<Todo>();
        Assert.Equal("Updated", result!.Name);
        Assert.True(result.IsComplete);
    }

    [Fact]
    public async Task Delete_WithExistingTodo_ReturnsNoContent()
    {
        var token = await GetTokenAsync(Role.Admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create todo
        var todo = new Todo { Name = "To Delete", IsComplete = false };
        var createResponse = await _client.PostAsJsonAsync("/todoitems", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Delete
        var response = await _client.DeleteAsync($"/todoitems/{createdTodo!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify
        var getResponse = await _client.GetAsync($"/todoitems/{createdTodo.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task GetComplete_ReturnsOnlyCompletedTodos()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create mixed todos
        await _client.PostAsJsonAsync("/todoitems", new Todo { Name = "Incomplete", IsComplete = false });
        await _client.PostAsJsonAsync("/todoitems", new Todo { Name = "Complete 1", IsComplete = true });
        await _client.PostAsJsonAsync("/todoitems", new Todo { Name = "Complete 2", IsComplete = true });

        var response = await _client.GetAsync("/todoitems/complete");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
        Assert.NotNull(todos);
        Assert.All(todos, t => Assert.True(t.IsComplete));
    }

    [Fact]
    public async Task Delete_WithNonAdminRole_ReturnsForbidden()
    {
        var token = await GetTokenAsync(Role.Parent);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a todo
        var todo = new Todo { Name = "To Delete", IsComplete = false };
        var createResponse = await _client.PostAsJsonAsync("/todoitems", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Try to delete
        var response = await _client.DeleteAsync($"/todoitems/{createdTodo!.Id}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}

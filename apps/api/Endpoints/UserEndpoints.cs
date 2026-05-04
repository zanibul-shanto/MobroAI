using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;

namespace MorboLensAI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        // --- Write Operations ---
        group.MapPost("/", Create);                 // POST   /users
        group.MapPut("/{id}", Update);              // PUT    /users/{id}
        group.MapDelete("/{id}", Delete);           // DELETE /users/{id}

        // --- Read Operations ---
        group.MapGet("/{id}", GetById);             // GET    /users/{id}
        group.MapGet("/", GetAll);                  // GET    /users
    }

    // --- Handler Implementations ---

    private static async Task<IResult> Create(User user, AppDbContext db)
    {
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Results.Created($"/users/{user.Id}", user);
    }

    private static async Task<IResult> Update(Guid id, User inputUser, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);

        if (user is null) return Results.NotFound();

        user.Email = inputUser.Email;
        if (!string.IsNullOrEmpty(inputUser.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(inputUser.PasswordHash);
        }
        user.FullName = inputUser.FullName;
        user.PhoneNumber = inputUser.PhoneNumber;
        user.Role = inputUser.Role;
        user.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);

        if (user is null) return Results.NotFound();

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return Results.NotFound();
        
        var userDto = new UserDto(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role);
        return Results.Ok(userDto);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var users = await db.Users
            .Select(user => new UserDto(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role))
            .ToListAsync();
        return Results.Ok(users);
    }
}
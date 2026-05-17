using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MobroLens.Models;

namespace MobroLens.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        // --- Write Operations ---
        group.MapPost("/", Create);
        group.MapPut("/{id}", Update).RequireAuthorization();
        group.MapDelete("/{id}", Delete).RequireAuthorization();
        group.MapPost("/change-password", ChangePassword).RequireAuthorization();

        // --- Read Operations ---
        group.MapGet("/{id}", GetById);
        group.MapGet("/", GetAll);
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

    private static async Task<IResult> ChangePassword(ChangePasswordRequest request, ClaimsPrincipal userClaims, AppDbContext db)
    {
        var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
        {
            return Results.Unauthorized();
        }

        var user = await db.Users.FindAsync(userId);
        if (user == null) return Results.NotFound("User not found.");

        if (request.NewPassword != request.ConfirmPassword)
        {
            return Results.BadRequest(new { message = "Passwords do not match." });
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.Ok(new { message = "Password changed successfully." });
    }
}
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
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Results.Created($"/users/{user.Id}", user);
    }

    private static async Task<IResult> Update(int id, User inputUser, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);

        if (user is null) return Results.NotFound();

        user.Email = inputUser.Email;
        user.Password = inputUser.Password;
        user.FirstName = inputUser.FirstName;
        user.LastName = inputUser.LastName;
        user.Location = inputUser.Location;
        user.UserType = inputUser.UserType;
        user.MobileNo = inputUser.MobileNo;
        user.NID = inputUser.NID;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(int id, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);

        if (user is null) return Results.NotFound();

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> GetById(int id, AppDbContext db)
    {
        var user = await db.Users.FindAsync(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        return Results.Ok(await db.Users.ToListAsync());
    }
}
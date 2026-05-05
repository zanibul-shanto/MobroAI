using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;

namespace MorboLensAI.Endpoints;

public static class ChildEndpoints
{
    public static void MapChildEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/children").RequireAuthorization();

        // --- Write Operations ---
        group.MapPost("/", Create);
        group.MapPut("/{id}", Update);
        group.MapDelete("/{id}", Delete);

        // --- Read Operations ---
        group.MapGet("/{id}", GetById);
        group.MapGet("/", GetAll);
        group.MapGet("/parent/{parentId}", GetByParent);
    }

    // --- Handler Implementations ---

    private static async Task<IResult> Create(Child child, AppDbContext db)
    {
        db.Children.Add(child);
        await db.SaveChangesAsync();
        return Results.Created($"/children/{child.Id}", child);
    }

    private static async Task<IResult> Update(Guid id, Child inputChild, AppDbContext db)
    {
        var child = await db.Children.FindAsync(id);

        if (child is null) return Results.NotFound();

        child.FullName = inputChild.FullName;
        child.DateOfBirth = inputChild.DateOfBirth;
        child.Gender = inputChild.Gender;
        child.ParentId = inputChild.ParentId;
        child.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var child = await db.Children.FindAsync(id);

        if (child is null) return Results.NotFound();

        db.Children.Remove(child);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var child = await db.Children.FindAsync(id);
        return child is null ? Results.NotFound() : Results.Ok(child);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        return Results.Ok(await db.Children.ToListAsync());
    }

    private static async Task<IResult> GetByParent(Guid parentId, AppDbContext db)
    {
        var children = await db.Children.Where(c => c.ParentId == parentId).ToListAsync();
        return Results.Ok(children);
    }
}

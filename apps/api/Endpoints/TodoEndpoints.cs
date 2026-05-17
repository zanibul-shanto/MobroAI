using Microsoft.EntityFrameworkCore;
using MobroLens.Models;

namespace MobroLens.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/todoitems").RequireAuthorization();

        // --- Write Operations ---
        group.MapPost("/", Create);
        group.MapPut("/{id}", Update);
        group.MapDelete("/{id}", Delete).RequireAuthorization(p => p.RequireRole("Admin"));

        // --- Read Operations ---
        group.MapGet("/{id}", GetById);
        group.MapGet("/", GetAll);

        // --- Other Operations ---
        group.MapGet("/complete", GetComplete);
    }

    // --- Handler Implementations ---

    private static async Task<IResult> Create(Todo todo, AppDbContext db)
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return Results.Created($"/todoitems/{todo.Id}", todo);
    }

    private static async Task<IResult> Update(int id, Todo inputTodo, AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return Results.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(int id, AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return Results.NotFound();

        db.Todos.Remove(todo);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> GetById(int id, AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);
        return todo is null ? Results.NotFound() : Results.Ok(todo);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        return Results.Ok(await db.Todos.ToListAsync());
    }

    // --- Other Handler Implementations ---
    private static async Task<IResult> GetComplete(AppDbContext db)
    {
        return Results.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
    }
}

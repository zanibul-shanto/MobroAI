using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;

namespace MorboLensAI.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/todoitems");

        // --- CRUD Operations ---
        group.MapGet("/", GetAllTodos);                 // GET /todoitems
        group.MapGet("/{id}", GetTodoById);             // GET /todoitems/{id}
        group.MapPost("/", CreateTodo);                 // POST /todoitems
        group.MapPut("/{id}", UpdateTodo);              // PUT /todoitems/{id}
        group.MapDelete("/{id}", DeleteTodo);           // DELETE /todoitems/{id}

        // --- Other Operations ---
        group.MapGet("/complete", GetCompleteTodos);    // GET /todoitems/complete
    }

    // --- Handler Implementations ---

    private static async Task<IResult> GetAllTodos(AppDbContext db)
    {
        return Results.Ok(await db.Todos.ToListAsync());
    }

    private static async Task<IResult> GetTodoById(int id, AppDbContext db)
    {
        return await db.Todos.FindAsync(id) is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound();
    }

    private static async Task<IResult> CreateTodo(Todo todo, AppDbContext db)
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return Results.Created($"/todoitems/{todo.Id}", todo);
    }

    private static async Task<IResult> UpdateTodo(int id, Todo inputTodo, AppDbContext db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return Results.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> DeleteTodo(int id, AppDbContext db)
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    private static async Task<IResult> GetCompleteTodos(AppDbContext db)
    {
        return Results.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
    }
}

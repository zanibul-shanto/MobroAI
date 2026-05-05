using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;
using System.Security.Claims;

namespace MorboLensAI.Endpoints;

public static class LocationLogEndpoints
{
    public static void MapLocationLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/location-logs").RequireAuthorization();

        // --- Write Operations ---
        group.MapPost("/", Create);

        // --- Read Operations ---
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapGet("/user/{userId}", GetByUserId);
        group.MapGet("/my-logs", GetMyLogs);
    }

    // --- Handler Implementations ---

    private static async Task<IResult> Create(LocationLog log, ClaimsPrincipal userClaims, AppDbContext db)
    {
        var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString != null && Guid.TryParse(userIdString, out var userId))
        {
            log.UserId = userId;
        }

        db.LocationLogs.Add(log);
        await db.SaveChangesAsync();
        return Results.Created($"/location-logs/{log.Id}", log);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        return Results.Ok(await db.LocationLogs.ToListAsync());
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var log = await db.LocationLogs.FindAsync(id);
        return log is null ? Results.NotFound() : Results.Ok(log);
    }

    private static async Task<IResult> GetByUserId(Guid userId, AppDbContext db)
    {
        var logs = await db.LocationLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
        return Results.Ok(logs);
    }

    private static async Task<IResult> GetMyLogs(ClaimsPrincipal userClaims, AppDbContext db)
    {
        var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
        {
            return Results.Unauthorized();
        }

        var logs = await db.LocationLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
        return Results.Ok(logs);
    }
}

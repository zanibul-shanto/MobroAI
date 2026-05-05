using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;
using System.Security.Claims;

namespace MorboLensAI.Endpoints;

public static class MeaslesScanEndpoints
{
    public static void MapMeaslesScanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/scans").RequireAuthorization();

        // --- Write Operations ---
        group.MapPost("/", Create);
        group.MapPut("/{id}/status", UpdateStatus);
        group.MapPost("/{id}/photos", AddPhoto);

        // --- Read Operations ---
        group.MapGet("/{id}", GetById);
        group.MapGet("/", GetAll);
        group.MapGet("/child/{childId}", GetByChild);
        group.MapGet("/user/{userId}", GetByUser);
        group.MapGet("/{id}/photos", GetPhotos);
    }

    // --- Handler Implementations ---

    private static async Task<IResult> Create(MeaslesScan scan, ClaimsPrincipal userClaims, AppDbContext db)
    {
        var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString != null && Guid.TryParse(userIdString, out var userId))
        {
            scan.UploadedById = userId;
        }

        db.MeaslesScans.Add(scan);
        await db.SaveChangesAsync();
        return Results.Created($"/scans/{scan.Id}", scan);
    }

    private static async Task<IResult> UpdateStatus(Guid id, ScanStatus status, AppDbContext db)
    {
        var scan = await db.MeaslesScans.FindAsync(id);
        if (scan is null) return Results.NotFound();

        scan.Status = status;
        scan.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> AddPhoto(Guid id, ScanPhoto photo, AppDbContext db)
    {
        var scan = await db.MeaslesScans.FindAsync(id);
        if (scan is null) return Results.NotFound("Scan not found.");

        photo.ScanId = id;
        db.ScanPhotos.Add(photo);
        await db.SaveChangesAsync();
        return Results.Created($"/scans/{id}/photos/{photo.Id}", photo);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var scan = await db.MeaslesScans.FindAsync(id);
        return scan is null ? Results.NotFound() : Results.Ok(scan);
    }

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        return Results.Ok(await db.MeaslesScans.ToListAsync());
    }

    private static async Task<IResult> GetByChild(Guid childId, AppDbContext db)
    {
        var scans = await db.MeaslesScans
            .Where(s => s.ChildId == childId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        return Results.Ok(scans);
    }

    private static async Task<IResult> GetByUser(Guid userId, AppDbContext db)
    {
        var scans = await db.MeaslesScans
            .Where(s => s.UploadedById == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        return Results.Ok(scans);
    }

    private static async Task<IResult> GetPhotos(Guid id, AppDbContext db)
    {
        var photos = await db.ScanPhotos
            .Where(p => p.ScanId == id)
            .ToListAsync();
        return Results.Ok(photos);
    }
}

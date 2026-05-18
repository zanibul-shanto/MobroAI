using Microsoft.EntityFrameworkCore;
using MobroLens.Models;

namespace MobroLens.Endpoints;

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

        // --- Vaccine Date Operations ---
        group.MapPost("/{childId}/vaccines", AddVaccineDate);
        group.MapPut("/{childId}/vaccines/{id}", UpdateVaccineDate);
        group.MapDelete("/{childId}/vaccines/{id}", DeleteVaccineDate);
        group.MapGet("/{childId}/vaccines", GetVaccineDates);
        group.MapGet("/parent/{parentId}/upcoming-vaccines", GetUpcomingVaccines);
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

    // --- Vaccine Date Handlers ---

    private static async Task<IResult> AddVaccineDate(Guid childId, VaccineDate input, AppDbContext db)
    {
        var child = await db.Children.FindAsync(childId);
        if (child is null) return Results.NotFound();

        var vaccineDate = new VaccineDate
        {
            ChildId = childId,
            Date = input.Date,
            Note = input.Note
        };

        db.VaccineDates.Add(vaccineDate);
        await db.SaveChangesAsync();
        return Results.Created($"/children/{childId}/vaccines/{vaccineDate.Id}", vaccineDate);
    }

    private static async Task<IResult> UpdateVaccineDate(Guid childId, Guid id, VaccineDate input, AppDbContext db)
    {
        var vaccineDate = await db.VaccineDates.FirstOrDefaultAsync(v => v.Id == id && v.ChildId == childId);
        if (vaccineDate is null) return Results.NotFound();

        vaccineDate.Date = input.Date;
        vaccineDate.Note = input.Note;
        vaccineDate.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteVaccineDate(Guid childId, Guid id, AppDbContext db)
    {
        var vaccineDate = await db.VaccineDates.FirstOrDefaultAsync(v => v.Id == id && v.ChildId == childId);
        if (vaccineDate is null) return Results.NotFound();

        db.VaccineDates.Remove(vaccineDate);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> GetVaccineDates(Guid childId, AppDbContext db)
    {
        var child = await db.Children.FindAsync(childId);
        if (child is null) return Results.NotFound();

        var dates = await db.VaccineDates
            .Where(v => v.ChildId == childId)
            .OrderBy(v => v.Date)
            .ToListAsync();

        return Results.Ok(dates);
    }

    private static async Task<IResult> GetUpcomingVaccines(Guid parentId, AppDbContext db, int limit = 3)
    {
        var today = DateTime.UtcNow.Date;

        var upcoming = await db.Children
            .Where(c => c.ParentId == parentId)
            .Join(db.VaccineDates,
                c => c.Id,
                v => v.ChildId,
                (c, v) => new { c.FullName, v.ChildId, v.Date, v.Note })
            .Where(x => x.Date.Date >= today)
            .OrderBy(x => x.Date)
            .Take(limit)
            .ToListAsync();

        return Results.Ok(upcoming);
    }
}

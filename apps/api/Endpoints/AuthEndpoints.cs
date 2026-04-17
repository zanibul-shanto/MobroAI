using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;
using MorboLensAI.Services;

namespace MorboLensAI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
    }

    private static async Task<IResult> Register(RegisterRequest request, AppDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Username == request.Username))
            return Results.BadRequest("Username already exists.");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Results.Ok("User registered successfully.");
    }

    private static async Task<IResult> Login(LoginRequest request, AppDbContext db, ITokenService tokenService)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Results.Unauthorized();

        var accessToken = tokenService.GenerateAccessToken(user);

        return Results.Ok(new AuthResponse(accessToken));
    }
}

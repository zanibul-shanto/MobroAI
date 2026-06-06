using Microsoft.EntityFrameworkCore;
using MobroLens.Models;
using MobroLens.Services;

namespace MobroLens.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/forgot-password", ForgotPassword);
        group.MapPost("/reset-password", ResetPassword);
    }

    private static async Task<IResult> Register(RegisterRequest request, AppDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email))
        {
            return Results.BadRequest("User with this email already exists.");
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber) && await db.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
        {
            return Results.BadRequest("User with this phone number already exists.");
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Results.Ok("User created successfully.");
    }

    private static async Task<IResult> Login(LoginRequest request, AppDbContext db, ITokenService tokenService)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Identifier || u.PhoneNumber == request.Identifier);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        var token = tokenService.GenerateAccessToken(user);
        var userDto = new UserDto(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role);

        return Results.Ok(new AuthResponse(token, userDto));
    }

    private static async Task<IResult> ForgotPassword(ForgotPasswordRequest request, AppDbContext db, IEmailService emailService)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Identifier || u.PhoneNumber == request.Identifier);

        if (user != null)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            user.PasswordResetCode = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await db.SaveChangesAsync();

            await emailService.SendPasswordResetAsync(user.Email, code);
        }

        return Results.Ok("If an account with that email/phone exists, a verification code has been sent.");
    }

    private static async Task<IResult> ResetPassword(ResetPasswordRequest request, AppDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Identifier || u.PhoneNumber == request.Identifier);

        if (user == null)
            return Results.NotFound("User not found.");

        if (user.PasswordResetCode != request.Code || user.PasswordResetCodeExpiry < DateTime.UtcNow)
            return Results.BadRequest("Invalid or expired verification code.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetCode = null;
        user.PasswordResetCodeExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Results.Ok("Password reset successfully.");
    }
}

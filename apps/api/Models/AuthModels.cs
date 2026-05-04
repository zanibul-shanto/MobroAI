namespace MorboLensAI.Models;

public record LoginRequest(string Identifier, string Password);
public record RegisterRequest(string Email, string Password, string FullName, string? PhoneNumber, Role Role);
public record UserDto(Guid Id, string Email, string FullName, string? PhoneNumber, Role Role);
public record AuthResponse(string AccessToken, UserDto User);
public record ForgotPasswordRequest(string Identifier);
public record ResetPasswordRequest(string Identifier, string Code, string NewPassword);

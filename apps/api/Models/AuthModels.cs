namespace MorboLensAI.Models;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, string FullName, Role Role);
public record AuthResponse(string AccessToken);

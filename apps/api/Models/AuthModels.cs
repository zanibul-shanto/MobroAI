namespace MorboLensAI.Models;

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password);
public record AuthResponse(string AccessToken);

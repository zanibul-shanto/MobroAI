using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class AuthModelsTests
{
    [Fact]
    public void LoginRequest_CanBeCreated()
    {
        var request = new LoginRequest("user@example.com", "password123");

        Assert.Equal("user@example.com", request.Identifier);
        Assert.Equal("password123", request.Password);
    }

    [Fact]
    public void RegisterRequest_CanBeCreated()
    {
        var request = new RegisterRequest(
            "user@example.com",
            "password123",
            "John Doe",
            "+1234567890",
            Role.Parent
        );

        Assert.Equal("user@example.com", request.Email);
        Assert.Equal("password123", request.Password);
        Assert.Equal("John Doe", request.FullName);
        Assert.Equal("+1234567890", request.PhoneNumber);
        Assert.Equal(Role.Parent, request.Role);
    }

    [Fact]
    public void RegisterRequest_WithNullPhoneNumber_Works()
    {
        var request = new RegisterRequest(
            "user@example.com",
            "password123",
            "John Doe",
            null,
            Role.HealthCareOfficer
        );

        Assert.Null(request.PhoneNumber);
        Assert.Equal(Role.HealthCareOfficer, request.Role);
    }

    [Fact]
    public void UserDto_CanBeCreated()
    {
        var id = Guid.NewGuid();
        var dto = new UserDto(id, "user@example.com", "John Doe", "+1234567890", Role.Parent);

        Assert.Equal(id, dto.Id);
        Assert.Equal("user@example.com", dto.Email);
        Assert.Equal("John Doe", dto.FullName);
        Assert.Equal("+1234567890", dto.PhoneNumber);
        Assert.Equal(Role.Parent, dto.Role);
    }

    [Fact]
    public void AuthResponse_CanBeCreated()
    {
        var user = new UserDto(Guid.NewGuid(), "user@example.com", "John Doe", null, Role.Parent);
        var response = new AuthResponse("jwt_token_here", user);

        Assert.Equal("jwt_token_here", response.AccessToken);
        Assert.Equal(user, response.User);
    }

    [Fact]
    public void ForgotPasswordRequest_CanBeCreated()
    {
        var request = new ForgotPasswordRequest("user@example.com");
        Assert.Equal("user@example.com", request.Identifier);
    }

    [Fact]
    public void ResetPasswordRequest_CanBeCreated()
    {
        var request = new ResetPasswordRequest("user@example.com", "12345", "newpassword");

        Assert.Equal("user@example.com", request.Identifier);
        Assert.Equal("12345", request.Code);
        Assert.Equal("newpassword", request.NewPassword);
    }

    [Fact]
    public void ChangePasswordRequest_CanBeCreated()
    {
        var request = new ChangePasswordRequest("newpassword", "newpassword");

        Assert.Equal("newpassword", request.NewPassword);
        Assert.Equal("newpassword", request.ConfirmPassword);
    }
}

using Microsoft.Extensions.Configuration;
using MobroLens.Models;
using MobroLens.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MobroLens.Tests.Services;

public class TokenServiceTests
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public TokenServiceTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "your-super-secret-key-min-32chars!!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:AccessTokenExpirationMinutes"] = "60"
            })
            .Build();

        _tokenService = new TokenService(_configuration);
    }

    [Fact]
    public void GenerateAccessToken_ReturnsNonEmptyString()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = Role.Parent
        };

        var token = _tokenService.GenerateAccessToken(user);

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public void GenerateAccessToken_ContainsValidClaims()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = Role.HealthCareOfficer
        };

        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(user.Role.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void GenerateAccessToken_HasCorrectIssuerAndAudience()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = Role.Admin
        };

        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal("TestIssuer", jwtToken.Issuer);
        Assert.Equal("TestAudience", jwtToken.Audiences.First());
    }

    [Fact]
    public void GenerateAccessToken_HasExpiration()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = Role.Parent
        };

        var beforeGeneration = DateTime.UtcNow;
        var token = _tokenService.GenerateAccessToken(user);
        var afterGeneration = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.True(jwtToken.ValidFrom >= beforeGeneration.AddSeconds(-5));
        Assert.True(jwtToken.ValidTo <= afterGeneration.AddMinutes(60).AddSeconds(5));
    }

    [Theory]
    [InlineData(Role.Admin)]
    [InlineData(Role.HealthCareOfficer)]
    [InlineData(Role.Parent)]
    public void GenerateAccessToken_HandlesAllRoles(Role role)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FullName = "Test User",
            Role = role
        };

        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(role.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }
}

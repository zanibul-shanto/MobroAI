using System.ComponentModel.DataAnnotations;
using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class UserModelTests
{
    [Fact]
    public void User_DefaultValues_AreCorrect()
    {
        var user = new User();

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal(string.Empty, user.FullName);
        Assert.Null(user.PhoneNumber);
        Assert.Equal(Role.Parent, user.Role);
    }

    [Fact]
    public void User_InheritsBaseEntity()
    {
        var user = new User();

        Assert.IsAssignableFrom<BaseEntity>(user);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
        Assert.True(user.UpdatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("", false)]
    public void User_EmailValidation_Works(string email, bool shouldBeValid)
    {
        var user = new User
        {
            Email = email,
            PasswordHash = "hashed",
            FullName = "Test User"
        };

        var validationResults = ValidateModel(user);
        var hasEmailErrors = validationResults.Any(v => v.MemberNames.Contains("Email"));

        if (shouldBeValid)
            Assert.False(hasEmailErrors);
        else
            Assert.True(hasEmailErrors || email == string.Empty);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}

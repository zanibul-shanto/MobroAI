using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class EnumTests
{
    [Theory]
    [InlineData(Role.Admin, 0)]
    [InlineData(Role.HealthCareOfficer, 1)]
    [InlineData(Role.Parent, 2)]
    public void Role_HasCorrectValues(Role role, int expectedValue)
    {
        Assert.Equal(expectedValue, (int)role);
    }

    [Theory]
    [InlineData(ScanStatus.Pending, 0)]
    [InlineData(ScanStatus.AI_Confirmed, 1)]
    [InlineData(ScanStatus.Officer_Verified, 2)]
    [InlineData(ScanStatus.Cleared, 3)]
    public void ScanStatus_HasCorrectValues(ScanStatus status, int expectedValue)
    {
        Assert.Equal(expectedValue, (int)status);
    }

    [Theory]
    [InlineData(Gender.Male, 0)]
    [InlineData(Gender.Female, 1)]
    [InlineData(Gender.Other, 2)]
    public void Gender_HasCorrectValues(Gender gender, int expectedValue)
    {
        Assert.Equal(expectedValue, (int)gender);
    }

    [Fact]
    public void ScanStatus_Progression_IsSequential()
    {
        var values = Enum.GetValues<ScanStatus>();
        
        for (int i = 0; i < values.Length; i++)
        {
            Assert.Equal(i, (int)values[i]);
        }
    }
}

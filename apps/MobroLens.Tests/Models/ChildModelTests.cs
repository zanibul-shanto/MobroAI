using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class ChildModelTests
{
    [Fact]
    public void Child_DefaultValues_AreCorrect()
    {
        var child = new Child();

        Assert.NotEqual(Guid.Empty, child.Id);
        Assert.Equal(Guid.Empty, child.ParentId);
        Assert.Equal(string.Empty, child.FullName);
        Assert.Equal(default(DateTime), child.DateOfBirth);
        Assert.Equal(default(Gender), child.Gender);
    }

    [Fact]
    public void Child_CanBeCreated_WithAllProperties()
    {
        var parentId = Guid.NewGuid();
        var dob = new DateTime(2020, 1, 15);

        var child = new Child
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            FullName = "John Doe",
            DateOfBirth = dob,
            Gender = Gender.Male
        };

        Assert.Equal("John Doe", child.FullName);
        Assert.Equal(parentId, child.ParentId);
        Assert.Equal(dob, child.DateOfBirth);
        Assert.Equal(Gender.Male, child.Gender);
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public void Child_GenderEnum_AcceptsAllValues(Gender gender)
    {
        var child = new Child { Gender = gender };
        Assert.Equal(gender, child.Gender);
    }
}

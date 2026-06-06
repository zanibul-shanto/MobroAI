using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class VaccineDateModelTests
{
    [Fact]
    public void VaccineDate_DefaultValues_AreCorrect()
    {
        var vaccine = new VaccineDate();

        Assert.NotEqual(Guid.Empty, vaccine.Id);
        Assert.Equal(Guid.Empty, vaccine.ChildId);
        Assert.Equal(default(DateTime), vaccine.Date);
        Assert.Null(vaccine.Note);
    }

    [Fact]
    public void VaccineDate_CanBeCreated_WithAllProperties()
    {
        var childId = Guid.NewGuid();
        var date = new DateTime(2024, 6, 15);

        var vaccine = new VaccineDate
        {
            Id = Guid.NewGuid(),
            ChildId = childId,
            Date = date,
            Note = "BCG Vaccine"
        };

        Assert.Equal(childId, vaccine.ChildId);
        Assert.Equal(date, vaccine.Date);
        Assert.Equal("BCG Vaccine", vaccine.Note);
    }
}

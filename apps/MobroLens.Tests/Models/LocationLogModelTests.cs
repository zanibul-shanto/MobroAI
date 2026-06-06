using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class LocationLogModelTests
{
    [Fact]
    public void LocationLog_DefaultValues_AreCorrect()
    {
        var log = new LocationLog();

        Assert.NotEqual(Guid.Empty, log.Id);
        Assert.Equal(Guid.Empty, log.UserId);
        Assert.Equal(0m, log.Latitude);
        Assert.Equal(0m, log.Longitude);
        Assert.False(log.WithChild);
    }

    [Fact]
    public void LocationLog_CanStoreCoordinates()
    {
        var log = new LocationLog
        {
            Latitude = 23.8103m,
            Longitude = 90.4125m,
            WithChild = true
        };

        Assert.Equal(23.8103m, log.Latitude);
        Assert.Equal(90.4125m, log.Longitude);
        Assert.True(log.WithChild);
    }
}

using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class ScanPhotoModelTests
{
    [Fact]
    public void ScanPhoto_DefaultValues_AreCorrect()
    {
        var photo = new ScanPhoto();

        Assert.NotEqual(Guid.Empty, photo.Id);
        Assert.Equal(Guid.Empty, photo.ScanId);
        Assert.NotNull(photo.ImageData);
        Assert.Empty(photo.ImageData);
    }

    [Fact]
    public void ScanPhoto_CanStoreImageData()
    {
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };

        var photo = new ScanPhoto
        {
            Id = Guid.NewGuid(),
            ScanId = Guid.NewGuid(),
            ImageData = imageBytes
        };

        Assert.Equal(imageBytes, photo.ImageData);
        Assert.Equal(4, photo.ImageData.Length);
    }
}

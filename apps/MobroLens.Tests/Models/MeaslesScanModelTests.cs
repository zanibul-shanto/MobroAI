using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class MeaslesScanModelTests
{
    [Fact]
    public void MeaslesScan_DefaultValues_AreCorrect()
    {
        var scan = new MeaslesScan();

        Assert.NotEqual(Guid.Empty, scan.Id);
        Assert.Equal(Guid.Empty, scan.ChildId);
        Assert.Equal(Guid.Empty, scan.UploadedById);
        Assert.Null(scan.AnalysisResultJson);
        Assert.Equal(0m, scan.ConfidenceScore);
        Assert.Null(scan.Latitude);
        Assert.Null(scan.Longitude);
        Assert.Equal(ScanStatus.Pending, scan.Status);
    }

    [Fact]
    public void MeaslesScan_StatusTransitions_Work()
    {
        var scan = new MeaslesScan();

        Assert.Equal(ScanStatus.Pending, scan.Status);

        scan.Status = ScanStatus.AI_Confirmed;
        Assert.Equal(ScanStatus.AI_Confirmed, scan.Status);

        scan.Status = ScanStatus.Officer_Verified;
        Assert.Equal(ScanStatus.Officer_Verified, scan.Status);

        scan.Status = ScanStatus.Cleared;
        Assert.Equal(ScanStatus.Cleared, scan.Status);
    }

    [Fact]
    public void MeaslesScan_CanStoreLocation()
    {
        var scan = new MeaslesScan
        {
            Latitude = 23.8103m,
            Longitude = 90.4125m
        };

        Assert.Equal(23.8103m, scan.Latitude);
        Assert.Equal(90.4125m, scan.Longitude);
    }

    [Fact]
    public void MeaslesScan_CanStoreAnalysisResult()
    {
        var json = "{\"Measles\": 95.5, \"Normal\": 4.5}";
        var scan = new MeaslesScan
        {
            AnalysisResultJson = json,
            ConfidenceScore = 95.5m
        };

        Assert.Equal(json, scan.AnalysisResultJson);
        Assert.Equal(95.5m, scan.ConfidenceScore);
    }
}

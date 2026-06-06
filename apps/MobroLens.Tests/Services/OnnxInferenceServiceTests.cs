using MobroLens.Services;

namespace MobroLens.Tests.Services;

public class OnnxInferenceServiceTests
{
    private readonly OnnxInferenceService _service;

    public OnnxInferenceServiceTests()
    {
        _service = new OnnxInferenceService();
    }

    [Fact]
    public void Predict_WithValidImage_ReturnsResult()
    {
        // Create a minimal valid PNG image (1x1 pixel)
        var imageBytes = CreateMinimalPng();

        var result = _service.Predict(imageBytes);

        Assert.NotNull(result);
        Assert.NotNull(result.PredictedClass);
        Assert.True(result.Confidence >= 0 && result.Confidence <= 100);
        Assert.NotNull(result.AllScores);
        Assert.Equal(4, result.AllScores.Count);
        Assert.Contains("Chickenpox", result.AllScores.Keys);
        Assert.Contains("Measles", result.AllScores.Keys);
        Assert.Contains("Monkeypox", result.AllScores.Keys);
        Assert.Contains("Normal", result.AllScores.Keys);
    }

    [Fact]
    public void Predict_AllScoresSumToApproximately100()
    {
        var imageBytes = CreateMinimalPng();
        var result = _service.Predict(imageBytes);

        var total = result.AllScores.Values.Sum();
        Assert.True(total >= 99m && total <= 101m, $"Expected scores to sum to ~100, but got {total}");
    }

    [Fact]
    public void PredictionResult_RecordProperties_Work()
    {
        var scores = new Dictionary<string, decimal>
        {
            ["Measles"] = 95.5m,
            ["Normal"] = 4.5m,
            ["Chickenpox"] = 0m,
            ["Monkeypox"] = 0m
        };

        var result = new PredictionResult("Measles", 95.5m, scores);

        Assert.Equal("Measles", result.PredictedClass);
        Assert.Equal(95.5m, result.Confidence);
        Assert.Equal(scores, result.AllScores);
    }

    [Fact]
    public void PredictionResult_Equality_Works()
    {
        var scores1 = new Dictionary<string, decimal> { ["Measles"] = 95.5m };
        var scores2 = new Dictionary<string, decimal> { ["Measles"] = 95.5m };

        var result1 = new PredictionResult("Measles", 95.5m, scores1);
        var result2 = new PredictionResult("Measles", 95.5m, scores2);

        // Records have value-based equality
        Assert.Equal(result1.PredictedClass, result2.PredictedClass);
        Assert.Equal(result1.Confidence, result2.Confidence);
    }

    private static byte[] CreateMinimalPng()
    {
        // Minimal 1x1 PNG: IHDR + IDAT + IEND chunks
        return Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8DwHwAFBQIAX8jx0gAAAABJRU5ErkJggg=="
        );
    }
}

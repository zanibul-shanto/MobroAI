using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;

namespace MobroLens.Services;

public class OnnxInferenceService
{
    private static readonly string[] Labels = ["Chickenpox", "Measles", "Monkeypox", "Normal"];
    private readonly InferenceSession _session;

    public OnnxInferenceService()
    {
        var modelPath = Path.Combine(AppContext.BaseDirectory, "ML", "monkeynet_lite_original.onnx");
        _session = new InferenceSession(modelPath);
    }

    public PredictionResult Predict(byte[] imageBytes)
    {
        using var image = Image.Load<Rgb24>(imageBytes);
        image.Mutate(x => x.Resize(224, 224));

        // NHWC tensor [1, 224, 224, 3] — Keras channels-last format
        var tensor = new DenseTensor<float>([1, 224, 224, 3]);
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < 224; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < 224; x++)
                {
                    // MobileNetV2 standard normalization: pixel → [-1, 1]
                    tensor[0, y, x, 0] = (row[x].R / 127.5f) - 1f;
                    tensor[0, y, x, 1] = (row[x].G / 127.5f) - 1f;
                    tensor[0, y, x, 2] = (row[x].B / 127.5f) - 1f;
                }
            }
        });

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_layer_6", tensor)
        };

        using var results = _session.Run(inputs);
        // Model output is already softmax probabilities
        var scores = results.First().AsEnumerable<float>().ToArray();

        var maxIdx = Array.IndexOf(scores, scores.Max());

        return new PredictionResult(
            PredictedClass: Labels[maxIdx],
            Confidence: (decimal)Math.Round(scores[maxIdx] * 100, 2),
            AllScores: Labels.Zip(scores).ToDictionary(p => p.First, p => (decimal)Math.Round(p.Second * 100, 2))
        );
    }
}

public record PredictionResult(
    string PredictedClass,
    decimal Confidence,
    Dictionary<string, decimal> AllScores
);

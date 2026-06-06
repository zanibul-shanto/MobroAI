using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MobroLens.Services;

namespace MobroLens.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Switches Program.cs to UseInMemoryDatabase — avoids the two-provider conflict
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // Replace real ONNX service with a fast fake (avoids loading 9.4MB model for unrelated tests)
            var onnxDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IOnnxInferenceService));
            if (onnxDescriptor != null) services.Remove(onnxDescriptor);

            services.AddSingleton<IOnnxInferenceService>(new FakeOnnxInferenceService());
        });
    }
}

internal class FakeOnnxInferenceService : IOnnxInferenceService
{
    public PredictionResult Predict(byte[] imageBytes) => new(
        "Normal", 99m,
        new Dictionary<string, decimal>
        {
            ["Chickenpox"] = 0m,
            ["Measles"] = 0m,
            ["Monkeypox"] = 0m,
            ["Normal"] = 99m
        });
}

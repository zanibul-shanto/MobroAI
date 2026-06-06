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
            var onnxDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IOnnxInferenceService));
            if (onnxDescriptor != null) services.Remove(onnxDescriptor);
            services.AddSingleton<IOnnxInferenceService>(new FakeOnnxInferenceService());

            // Replace real email service with a no-op fake (no API key needed in tests)
            var emailDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IEmailService));
            if (emailDescriptor != null) services.Remove(emailDescriptor);
            services.AddScoped<IEmailService>(_ => new FakeEmailService());
        });
    }
}

internal class FakeEmailService : IEmailService
{
    public Task SendPasswordResetAsync(string toEmail, string code) => Task.CompletedTask;
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

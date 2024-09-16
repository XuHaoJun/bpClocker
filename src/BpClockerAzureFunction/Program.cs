using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BpClockerAzureFunction.Dtos;
using Microsoft.Extensions.Configuration;
using BpClockerAzureFunction.Interfaces;
using BpClockerAzureFunction.Services;

var builder = new HostBuilder();
builder
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.Configure<BpConfigs>(configuration.GetSection("Bp"));

        // services
        services.AddScoped<IClockService, ClockService>();
    });

var host = builder.Build();
host.Run();

using Azure.Monitor.OpenTelemetry.Exporter;
using Ecommerce.Functions.Extensions;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// 1. Initialize the application builder
var builder = FunctionsApplication.CreateBuilder(args);
builder.Logging.AddFilter("Microsoft.Azure.Functions.Worker", LogLevel.Warning);

// Register all FluentValidators found in the current assembly
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 2. Configure the worker to handle HTTP requests properly
builder.ConfigureFunctionsWebApplication();

// 3. Configure built-in Azure logging/telemetry
builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

// 4. Delegate all custom dependency injection to a separate method
builder.Services.RegisterConfigurations(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

// 5. Build and run the host
await builder.Build().RunAsync();
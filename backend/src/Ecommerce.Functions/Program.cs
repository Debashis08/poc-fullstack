using Azure.Monitor.OpenTelemetry.AspNetCore;
using Ecommerce.Functions.Extensions;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// 1. Initialize the application builder
var builder = FunctionsApplication.CreateBuilder(args);

// Register all FluentValidators found in the current assembly
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 2. Configure the worker to handle HTTP requests properly
builder.ConfigureFunctionsWebApplication();

// 3. Configure built-in Azure logging/telemetry
builder.Services.AddOpenTelemetry().UseAzureMonitor();

// 4. Delegate all custom dependency injection to a separate method
builder.Services.RegisterConfigurations(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

// 5. Build and run the host
await builder.Build().RunAsync();
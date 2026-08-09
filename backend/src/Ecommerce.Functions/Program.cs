using Ecommerce.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Poc.Ecommerce.Functions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. Initialize the application builder
            var builder = FunctionsApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            // 2. Configure the worker to handle HTTP requests properly
            builder.ConfigureFunctionsWebApplication();

            // 3. Configure built-in Azure logging/telemetry
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();

            // 4. Delegate all custom dependency injection to a separate method
            services.RegisterServices(configuration);

            // 5. Build and run the host
            builder.Build().Run();
        }
    }
}
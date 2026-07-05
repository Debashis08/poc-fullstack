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

            // 2. Configure the worker to handle HTTP requests properly
            builder.ConfigureFunctionsWebApplication();

            // 3. Configure built-in Azure logging/telemetry
            builder.Services.AddApplicationInsightsTelemetryWorkerService();
            builder.Services.ConfigureFunctionsApplicationInsights();

            // 4. Delegate all custom dependency injection to a separate method
            ConfigureServices(builder.Services);

            // 5. Build and run the host
            builder.Build().Run();
        }

        /// <summary>
        /// Centralized location for all custom Dependency Injection registrations.
        /// </summary>
        private static void ConfigureServices(IServiceCollection services)
        {
            // Future DI Registrations will go here! Examples:

            // services.AddScoped<ITokenService, TokenService>();
            // services.AddScoped<IOrderRepository, OrderRepository>();
            // services.AddDbContext<AppDbContext>(options => ...);
        }
    }
}
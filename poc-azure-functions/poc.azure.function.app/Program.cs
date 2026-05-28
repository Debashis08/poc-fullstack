using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace poc.azure.function.app;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();

        // Application Insights (Default)
        //builder.Services
        //    .AddApplicationInsightsTelemetryWorkerService()
        //    .ConfigureFunctionsApplicationInsights();

        // Register Entity Framework Core DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var sqlDbConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            options.UseSqlServer(sqlDbConnectionString);
        });

        // 2. Register all Messaging Clients and Infrastructure Services (Using your custom extension)
        builder.Services.AddServiceBusInfrastructure(builder.Configuration);

        // 3. Register your Integration layers
        builder.Services.AddScoped<II0001_1_LoginIntegration, I0001_1_LoginIntegration>();

        builder.Build().Run();
    }
}
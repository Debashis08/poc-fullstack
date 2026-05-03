using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using poc.azure.function.app.Services;

namespace poc.azure.function.app;

public static class ServiceBusRegistrationExtension
{
    public static IServiceCollection AddServiceBusInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Centralize all your queue and topic names here
        string[] serviceBusEntities = new[]
        {
            "sbq-dev",
            "sbq-test"
        };

        services.AddAzureClients(clientBuilder =>
        {
            // 2. Register the single underlying client connection pool
            string connectionString = configuration["StorageQueueConnectionString"]!;
            clientBuilder.AddServiceBusClient(connectionString);

            // 3. Loop through your list and register a named sender for each entity
            foreach (string entityName in serviceBusEntities)
            {
                clientBuilder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, provider) =>
                {
                    var client = provider.GetRequiredService<ServiceBusClient>();
                    return client.CreateSender(entityName);
                })
                // CRITICAL: We name the registered client using the exact queue/topic name
                .WithName(entityName);
            }
        });

        // Register your custom wrapper service
        services.AddScoped<IServiceBusService, ServiceBusService>();

        return services;
    }
}
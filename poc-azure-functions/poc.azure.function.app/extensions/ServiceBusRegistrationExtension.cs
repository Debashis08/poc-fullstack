using Azure.Storage.Queues; // 1. Switched from Azure.Messaging.ServiceBus to Azure.Storage.Queues
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
        // 1. Centralize all your queue names here (Note: Azurite does not support topics)
        string[] serviceBusEntities = new[]
        {
            "sbq-dev",
            "sbq-test"
        };

        services.AddAzureClients(clientBuilder =>
        {
            // 2. Register the single underlying client connection pool for Storage Queues
            string connectionString = configuration["StorageQueueConnectionString"]!;
            clientBuilder.AddQueueServiceClient(connectionString); // Switched to AddQueueServiceClient

            // 3. Loop through your list and register a named QueueClient for each entity
            foreach (string entityName in serviceBusEntities)
            {
                clientBuilder.AddClient<QueueClient, QueueClientOptions>((_, _, provider) =>
                {
                    // Resolve the top-level QueueServiceClient and create a specific QueueClient from it
                    var serviceClient = provider.GetRequiredService<QueueServiceClient>();
                    return serviceClient.GetQueueClient(entityName);
                })
                // CRITICAL: We name the registered client using the exact queue name
                .WithName(entityName);
            }
        });

        // Register your custom wrapper service
        services.AddScoped<IServiceBusService, ServiceBusService>();

        return services;
    }
}
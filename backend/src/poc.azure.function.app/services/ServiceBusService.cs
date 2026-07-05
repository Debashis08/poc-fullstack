using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using System.Text.Json;

namespace poc.azure.function.app.Services;

public class ServiceBusService : IServiceBusService
{
    private readonly IAzureClientFactory<QueueClient> _queueClientFactory;

    public ServiceBusService(IAzureClientFactory<QueueClient> queueClientFactory)
    {
        this._queueClientFactory = queueClientFactory;
    }

    public async Task SendMessageAsync(string queueOrTopicName, string message, string correlationId, IDictionary<string, object>? messageProperties = null, CancellationToken cancellationToken = default)
    {
        // Resolve the specific queue client from the factory
        var queueClient = this._queueClientFactory.CreateClient(queueOrTopicName);

        // Ensure the queue actually exists in your local Azurite emulator
        await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        // Because Azurite (Storage Queues) does not support native CorrelationId or ApplicationProperties,
        // we wrap your data into an anonymous JSON object so no information is lost.
        var azuritePayload = new
        {
            CorrelationId = correlationId,
            Properties = messageProperties,
            MessageBody = message
        };

        string serializedPayload = JsonSerializer.Serialize(azuritePayload);

        // Send the JSON string to Azurite
        await queueClient.SendMessageAsync(serializedPayload, cancellationToken).ConfigureAwait(false);
    }
}
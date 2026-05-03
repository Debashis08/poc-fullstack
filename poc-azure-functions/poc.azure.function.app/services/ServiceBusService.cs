using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace poc.azure.function.app.Services;

public class ServiceBusService : IServiceBusService
{
    private readonly IAzureClientFactory<ServiceBusSender> _serviceBusSender;

    public ServiceBusService(IAzureClientFactory<ServiceBusSender> serviceBusSender)
    {
        this._serviceBusSender = serviceBusSender;
    }

    public async Task SendMessageAsync(string queueOrTopicName, string message, string correlationId, IDictionary<string, object>? messageProperties = null, CancellationToken cancellationToken = default)
    {
        var serviceBusSender = this._serviceBusSender.CreateClient(queueOrTopicName);
        var serviceBusMessage = new ServiceBusMessage(message);

        if(!string.IsNullOrEmpty(correlationId))
        {
            serviceBusMessage.CorrelationId = correlationId;
        }

        if(messageProperties != null)
        {
            foreach (var property in messageProperties)
            {
                serviceBusMessage.ApplicationProperties.Add(property.Key, property.Value);
            }
        }

        await serviceBusSender.SendMessageAsync(serviceBusMessage, cancellationToken).ConfigureAwait(false);
    }
}
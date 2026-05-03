namespace poc.azure.function.app;

public interface IServiceBusService
{
    public Task SendMessageAsync(
        string queueOrTopicName,
        string message,
        string correlationId,
        IDictionary<string, object>? messageProperties = null,
        CancellationToken cancellationToken = default);
}
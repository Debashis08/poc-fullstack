namespace poc.azure.function.app;

public interface II0001_1_LoginIntegration
{
    public Task ProcessAndQueueMessageAsync(string queueName, string message, string correlationId, IDictionary<string, object> messageProperties, CancellationToken cancellationToken = default);
}


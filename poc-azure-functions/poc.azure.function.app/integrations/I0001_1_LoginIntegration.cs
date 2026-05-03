using Microsoft.Extensions.Logging;

namespace poc.azure.function.app;

public class I0001_1_LoginIntegration : II0001_1_LoginIntegration
{
    private readonly ILogger<I0001_1_LoginIntegration> _logger;
    private readonly IServiceBusService _serviceBusService;

    public I0001_1_LoginIntegration(ILogger<I0001_1_LoginIntegration> logger, IServiceBusService serviceBusService)
    {
        this._logger = logger;
        this._serviceBusService = serviceBusService;
    }

    public async Task ProcessAndQueueMessageAsync(string queueName, string message, string correlationId, IDictionary<string, object> messageProperties, CancellationToken cancellationToken = default)
    {
        this._logger.LogTrace($"{nameof(I0001_1_LoginIntegration)} - {nameof(ProcessAndQueueMessageAsync)} - started.");

        await this._serviceBusService.SendMessageAsync(queueName, message, correlationId, messageProperties, cancellationToken).ConfigureAwait(false);

        this._logger.LogTrace($"{nameof(I0001_1_LoginIntegration)} - {nameof(ProcessAndQueueMessageAsync)} - completed.");
    }
}

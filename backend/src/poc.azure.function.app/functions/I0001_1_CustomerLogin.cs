using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace poc.azure.function.app.functions;

public class I0001_1_CustomerLogin
{
    private readonly ILogger<I0001_1_CustomerLogin> _logger;
    private readonly II0001_1_LoginIntegration _loginIntegration;

    public I0001_1_CustomerLogin(ILogger<I0001_1_CustomerLogin> logger, II0001_1_LoginIntegration loginIntegration)
    {
        _logger = logger;
        _loginIntegration = loginIntegration;
    }

    [Function("I0001_1_CustomerLogin")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var queueOrTopicName = request.Headers["resourceName"];
        var message = await new StreamReader(request.Body).ReadToEndAsync().ConfigureAwait(false);
        var correlationId = request.Headers["correlationId"];

        await this._loginIntegration.ProcessAndQueueMessageAsync(queueOrTopicName!, message!, correlationId!, null!, cancellationToken).ConfigureAwait(false);


        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
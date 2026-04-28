using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace poc.azure.function.app.functions;

public class I0001_1_CustomerLogin
{
    private readonly ILogger<I0001_1_CustomerLogin> _logger;

    public I0001_1_CustomerLogin(ILogger<I0001_1_CustomerLogin> logger)
    {
        _logger = logger;
    }

    [Function("I0001_1_CustomerLogin")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
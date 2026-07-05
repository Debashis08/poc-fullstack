using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Functions.Functions;

public class I0001_UserLogin
{
    private readonly ILogger<I0001_UserLogin> _logger;

    public I0001_UserLogin(ILogger<I0001_UserLogin> logger)
    {
        _logger = logger;
    }

    [Function("I0001_UserLogin")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
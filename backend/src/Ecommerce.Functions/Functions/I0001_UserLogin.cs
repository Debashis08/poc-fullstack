using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Ecommerce.Functions.Functions;

public class I0001_UserLogin
{
    private readonly ILogger<I0001_UserLogin> _logger;

    public I0001_UserLogin(ILogger<I0001_UserLogin> logger)
    {
        _logger = logger;
    }

    [Function("I0001_UserLogin")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        JObject response = new JObject
        {
            ["message"] = "Welcome to Azure Functions!",
            ["status"] = "success"
        };

        var httpResponse = req.CreateResponse(HttpStatusCode.OK);

        await httpResponse.WriteStringAsync(response.ToString());

        return httpResponse;
    }
}
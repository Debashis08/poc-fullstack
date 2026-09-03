using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Ecommerce.Functions;

public class I0000_MockResponse
{
    private readonly ILogger<I0000_MockResponse> _logger;

    public I0000_MockResponse(ILogger<I0000_MockResponse> logger)
    {
        _logger = logger;
    }

    [Function("I0000_MockResponse")]
    public async Task<HttpResponseData> ProcessMockRequest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"{nameof(I0000_MockResponse)} - {nameof(ProcessMockRequest)} - started.");
        var queryParams = request.Query;
        var statusCode = queryParams["status"] ?? "500";
        var response = GetMockResponse(statusCode);
        _logger.LogTrace($"{nameof(I0000_MockResponse)} - {nameof(ProcessMockRequest)} - completed.");
        return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
    }

    private HttpResponseMessage GetMockResponse(string statusCode)
    {
        if (!int.TryParse(statusCode, out var status))
        {
            status = (int)HttpStatusCode.InternalServerError;
        }

        if (status < 100 || status > 599)
        {
            status = (int)HttpStatusCode.InternalServerError;
        }

        var response = new HttpResponseMessage((HttpStatusCode)status)
        {
            Content = new StringContent(
                $"{{\"statusCode\":{status},\"message\":\"Mock response for status code {status}\"}}",
                System.Text.Encoding.UTF8,
                "application/json")
        };

        return response;
    }
}
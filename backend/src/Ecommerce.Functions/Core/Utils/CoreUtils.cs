using Microsoft.Azure.Functions.Worker.Http;

namespace Ecommerce.Functions;

public static class CoreUtils
{
    public static async Task<HttpResponseData> ToHttpResponseDataAsync(HttpRequestData request, HttpResponseMessage responseMessage)
    {
        var response = request.CreateResponse(responseMessage.StatusCode);

        // Copy response headers
        foreach (var header in responseMessage.Headers)
        {
            response.Headers.Add(header.Key, string.Join(",", header.Value));
        }

        if (responseMessage.Content != null)
        {
            // Copy content headers
            foreach (var header in responseMessage.Content.Headers)
            {
                if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    continue;

                response.Headers.Add(header.Key, string.Join(",", header.Value));
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var responseBody = new
            {
                statusCode = (int)responseMessage.StatusCode,
                content = content
            };

            await response.WriteAsJsonAsync(responseBody);
        }

        return response;
    }
}
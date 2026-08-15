using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Functions;

public static class CoreUtils
{
    public static async Task<HttpResponseData> ToHttpResponseDataAsync(HttpRequestData request, HttpResponseMessage responseMessage)
    {
        var response = request.CreateResponse(responseMessage.StatusCode);

        foreach(var header in responseMessage.Headers)
        {
            response.Headers.Add(header.Key, string.Join(",", header.Value));
        }

        if(responseMessage.Content!= null)
        {
            foreach (var header in responseMessage.Headers)
            {
                response.Headers.Add(header.Key, string.Join(",", header.Value));
            }
            var content = await responseMessage.Content.ReadAsStringAsync();
            //var payload = new { content = content };
            await response.WriteStringAsync(content);
        }

        return response;
    }
}

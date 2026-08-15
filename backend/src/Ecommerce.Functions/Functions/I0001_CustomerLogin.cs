using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Ecommerce.Functions.Functions;

public class I0001_CustomerLogin
{
    private readonly ILogger<I0001_CustomerLogin> _logger;
    private readonly IValidator<Customer> _validator;

    public I0001_CustomerLogin(ILogger<I0001_CustomerLogin> logger, IValidator<Customer> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    [Function("CustomerLogin")]
    public async Task<HttpResponseData> ProcessRequest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - started");
        _logger.LogInformation($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - started");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        var customer = JsonSerializer.Deserialize<Customer>(requestBody);

        var validationResult = await _validator.ValidateAsync(customer!, cancellationToken);

        //if (!validationResult.IsValid)
        //{
        //    return new BadRequestObjectResult($"Invalid Request Body - {string.Join('|', validationResult.Errors)}");
        //}

        //_logger.LogTrace($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - finished");

        //return new OkObjectResult("ok");
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\r\n    \"name\": \"debashis\",\r\n    \"email\": \"debashisnandi@gmail.com\",\r\n    \"passwordHash\": \"87246358hhwsdfy39ther\"\r\n}")
        };
        _logger.LogTrace($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - finished");
        _logger.LogInformation($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - finished");

        return await CoreUtils.ToHttpResponseDataAsync(req, response).ConfigureAwait(false);
    }
}
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Ecommerce.Functions;

public class I0001_CustomerLogin
{
    private readonly ILogger<I0001_CustomerLogin> _logger;
    private readonly II0001_CustomerLoginIntegration _i0001_CustomerLoginIntegration;
    private readonly IValidator<Customer> _validator;

    public I0001_CustomerLogin(ILogger<I0001_CustomerLogin> logger, II0001_CustomerLoginIntegration i0001_CustomerLoginIntegration, IValidator<Customer> validator)
    {
        _logger = logger;
        _i0001_CustomerLoginIntegration = i0001_CustomerLoginIntegration;
        _validator = validator;
    }

    [Function("CustomerLogin")]
    public async Task<HttpResponseData> ProcessRequest([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - started");

        var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        var customer = JsonSerializer.Deserialize<Customer>(requestBody);

        var validationResult = await _validator.ValidateAsync(customer!, cancellationToken);
        var response = new HttpResponseMessage();
        if (!validationResult.IsValid)
        {
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent($"Invalid Request Body - {string.Join(" | ", validationResult.Errors)}")
            };

            return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
        }

        var result = _i0001_CustomerLoginIntegration.ProcessCustomerLogin(customer!);
        response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{result}")
        };

        _logger.LogInformation($"{nameof(I0001_CustomerLogin)} - {nameof(ProcessRequest)} - finished");

        return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
    }
}
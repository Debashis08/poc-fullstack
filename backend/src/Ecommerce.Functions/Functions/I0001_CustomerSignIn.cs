using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Ecommerce.Functions;

public class I0001_CustomerSignIn
{
    private readonly ILogger<I0001_CustomerSignIn> _logger;
    private readonly II0001_CustomerSignInIntegration _i0001_CustomerSignInIntegration;
    private readonly IValidator<CustomerSignInRequest> _validator;

    public I0001_CustomerSignIn(ILogger<I0001_CustomerSignIn> logger, II0001_CustomerSignInIntegration i0001_CustomerSignInIntegration, IValidator<CustomerSignInRequest> validator)
    {
        _logger = logger;
        _i0001_CustomerSignInIntegration = i0001_CustomerSignInIntegration;
        _validator = validator;
    }

    [Function("CustomerLogin")]
    public async Task<HttpResponseData> ProcessRequest([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(I0001_CustomerSignIn)} - {nameof(ProcessRequest)} - started");
        var response = new HttpResponseMessage();

        try
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            var customer = JsonSerializer.Deserialize<CustomerSignInRequest>(requestBody);

            var validationResult = await _validator.ValidateAsync(customer!, cancellationToken).ConfigureAwait(false);
            if (!validationResult.IsValid)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent($"Invalid Request Body - {string.Join(" | ", validationResult.Errors)}")
                };

                return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
            }

            var result = await _i0001_CustomerSignInIntegration.ProcessCustomerSignIn(customer!, cancellationToken).ConfigureAwait(false);
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{result}")
            };

            _logger.LogInformation($"{nameof(I0001_CustomerSignIn)} - {nameof(ProcessRequest)} - finished");

            return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
        }
        catch(UnauthorizedAccessException ex)
        {
            _logger.LogError($"{ex.Message}");
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(ex.Message)
            };

            return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(ex.Message)
            };

            return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
        }
    }
}
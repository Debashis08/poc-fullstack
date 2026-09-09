using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Ecommerce.Functions;

public class I0001_1_CustomerSignUp
{
    private readonly ILogger<I0001_1_CustomerSignUp> _logger;
    private readonly II0001_1_CustomerSignUpIntegration _i0001_1_CustomerSignUpIntegration;
    private readonly IValidator<CustomerSignUpRequest> _customerSignUpValidator;

    public I0001_1_CustomerSignUp(ILogger<I0001_1_CustomerSignUp> logger, II0001_1_CustomerSignUpIntegration i0001_1_CustomerSignUpIntegration, IValidator<CustomerSignUpRequest> customerSignUpValidator)
    {
        _logger = logger;
        _i0001_1_CustomerSignUpIntegration = i0001_1_CustomerSignUpIntegration;
        _customerSignUpValidator = customerSignUpValidator;
    }

    [Function("CustomerSignUp")]
    public async Task<HttpResponseData> ProcessCustomerSignUp([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(I0001_1_CustomerSignUp)} - {nameof(ProcessCustomerSignUp)} - started.");
        var response = new HttpResponseMessage();
        try
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync().ConfigureAwait(false);
            var customer = JsonSerializer.Deserialize<CustomerSignUpRequest>(requestBody);
            var validationResult = await _customerSignUpValidator.ValidateAsync(customer!, cancellationToken).ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent($"Invalid Request Body - {string.Join(" | ", validationResult.Errors)}")
                };

                return await CoreUtils.ToHttpResponseDataAsync(request, response).ConfigureAwait(false);
            }
            var tokenResponse = await _i0001_1_CustomerSignUpIntegration.ProcessCustomerSignUp(customer!, cancellationToken).ConfigureAwait(false);
            if (tokenResponse is not null)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"Customer profile created successfully.")
                };
                response.Headers.Add("AccessToken", tokenResponse.AccessToken);
                response.Headers.Add("RefreshToken", tokenResponse.RefreshToken);
            }
            else
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent($"Customer profile creation failed.")
                };
            }

            _logger.LogInformation($"{nameof(I0001_1_CustomerSignUp)} - {nameof(ProcessCustomerSignUp)} - completed.");
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
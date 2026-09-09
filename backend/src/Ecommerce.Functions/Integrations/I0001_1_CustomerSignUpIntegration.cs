using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace Ecommerce.Functions;

public class I0001_1_CustomerSignUpIntegration : II0001_1_CustomerSignUpIntegration
{
    private readonly ILogger<I0001_1_CustomerSignUpIntegration> _logger;
    private readonly ICustomerService _customerService;

    public I0001_1_CustomerSignUpIntegration(ILogger<I0001_1_CustomerSignUpIntegration> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    public async Task<TokenResponse> ProcessCustomerSignUp(CustomerSignUpRequest customer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{nameof(I0001_1_CustomerSignUpIntegration)} - {ProcessCustomerSignUp} - started.");
        var tokenResponse = await _customerService.CreateCustomerProfile(customer, cancellationToken).ConfigureAwait(false);
        if(tokenResponse == null)
        {
            _logger.LogWarning($"[WARNING] Customer provided email is already registered.");
            return null!;
        }

        _logger.LogInformation($"{nameof(I0001_1_CustomerSignUpIntegration)} - {ProcessCustomerSignUp} - completed.");
        return tokenResponse;
    }
}

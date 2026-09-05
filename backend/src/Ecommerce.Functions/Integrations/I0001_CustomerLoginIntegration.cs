using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class I0001_CustomerLoginIntegration : II0001_CustomerLoginIntegration
{
    private readonly ILogger<I0001_CustomerLoginIntegration> _iLogger;
    private readonly ITokenService _tokenService;

    public I0001_CustomerLoginIntegration(ILogger<I0001_CustomerLoginIntegration> iLogger, ITokenService tokenService)
    {
        _iLogger = iLogger;
        _tokenService = tokenService;
    }
    public string ProcessCustomerLogin(Customer customer)
    {
        _logger.LogInformation($"{nameof(I0001_CustomerLoginIntegration)} - {nameof(ProcessCustomerLogin)} - started");

        return _tokenService.GenerateToken(customer);

    }
}

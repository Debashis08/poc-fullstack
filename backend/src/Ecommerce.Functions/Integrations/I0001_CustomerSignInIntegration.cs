using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class I0001_CustomerSignInIntegration : II0001_CustomerSignInIntegration
{
    private readonly ILogger<I0001_CustomerSignInIntegration> _iLogger;
    private readonly IPasswordHasher<CustomerSignInRequest> _passwordHasher;
    private readonly ICustomerService _customerService;
    private readonly ITokenService _tokenService;

    public I0001_CustomerSignInIntegration(ILogger<I0001_CustomerSignInIntegration> iLogger, IPasswordHasher<CustomerSignInRequest> passwordHasher, ICustomerService customerService, ITokenService tokenService)
    {
        _iLogger = iLogger;
        _passwordHasher = passwordHasher;
        _customerService = customerService;
        _tokenService = tokenService;
    }
    public async Task<string> ProcessCustomerSignIn(CustomerSignInRequest customer, CancellationToken cancellationToken = default)
    {
        _iLogger.LogInformation($"{nameof(I0001_CustomerSignInIntegration)} - {nameof(ProcessCustomerSignIn)} - started.");
        var hashedPassword = await _customerService.GetUserPasswordHashByEmailAsync(customer.Email!, cancellationToken).ConfigureAwait(false);

        var verificationResult = _passwordHasher.VerifyHashedPassword(customer, hashedPassword, customer.Password!);

        if (verificationResult is PasswordVerificationResult.Failed)
        {
            _iLogger.LogWarning($"Invalid email or password");
            throw new UnauthorizedAccessException("Invalid emai or password");
        }

        var token = _tokenService.GenerateToken(customer);

        _iLogger.LogInformation($"{nameof(I0001_CustomerSignInIntegration)} - {nameof(ProcessCustomerSignIn)} - completed.");
        return token;
    }
}

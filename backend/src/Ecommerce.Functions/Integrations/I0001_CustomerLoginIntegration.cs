using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class I0001_CustomerLoginIntegration : II0001_CustomerLoginIntegration
{
    private readonly ILogger<I0001_CustomerLoginIntegration> _iLogger;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly ICustomerService _customerService;
    private readonly ITokenService _tokenService;

    public I0001_CustomerLoginIntegration(ILogger<I0001_CustomerLoginIntegration> iLogger, IPasswordHasher<Customer> passwordHasher, ICustomerService customerService, ITokenService tokenService)
    {
        _iLogger = iLogger;
        _passwordHasher = passwordHasher;
        _customerService = customerService;
        _tokenService = tokenService;
    }
    public async Task<string> ProcessCustomerLogin(Customer customer)
    {
        _iLogger.LogInformation($"{nameof(I0001_CustomerLoginIntegration)} - {nameof(ProcessCustomerLogin)} - started.");
        var hashedPassword = await _customerService.GetUserPasswordHashByEmailAsync(customer.Email!).ConfigureAwait(false);
        var verificationResult = _passwordHasher.VerifyHashedPassword(customer, hashedPassword, customer.PasswordHash!);

        if(verificationResult is PasswordVerificationResult.Failed)
        {
            _iLogger.LogWarning($"Invalid email or password");
            throw new UnauthorizedAccessException("Invalid emai or password");
        }

        var token = _tokenService.GenerateToken(customer);

        _iLogger.LogInformation($"{nameof(I0001_CustomerLoginIntegration)} - {nameof(ProcessCustomerLogin)} - completed.");

        return token;

    }
}

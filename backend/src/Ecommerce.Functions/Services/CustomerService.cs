using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Functions;

public class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<CustomerSignUpRequest> _passwordHasher;
    private readonly AppDbContext _dbContext;

    public CustomerService(ILogger<CustomerService> logger, IPasswordHasher<CustomerSignUpRequest> passwordHasher, ITokenService tokenService, AppDbContext dbContext)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    public async Task<TokenResponse> CreateCustomerProfile(CustomerSignUpRequest customer, CancellationToken cancellationToken = default)
    {
        // First check if the provided email already exists or not.
        var isEmailAlreadyRegistered = await _dbContext.Customers.Where(c => c.Email == customer.Email).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        // If the provided email is not yet registered, user can signup using the email.
        if (isEmailAlreadyRegistered == null)
        {
            // Create the user profile in db and return the tokens.
            var newCustomer = new Customer()
            {
                Email = customer.Email,
                PasswordHash = _passwordHasher.HashPassword(customer, customer.Password),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Customers.Add(newCustomer);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var tokenResponse = _tokenService.GenerateTokens(customer);
            var refreshTokenRecord = new CustomerRefreshToken()
            {
                CustomerId = newCustomer.CustomerId,
                // How to hash the refresh token here, using IPasswordHasher??
                TokenHash = _tokenService.HashRefreshToken(tokenResponse.RefreshToken),
                ExpiresAt = tokenResponse.RefreshTokenExpiresAt,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.CustomerRefreshTokens.Add(refreshTokenRecord);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return tokenResponse;
        }

        return null!;
    }

    public async Task<string> GetUserPasswordHashByEmailAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{nameof(CustomerService)} - {nameof(GetUserPasswordHashByEmailAsync)} - started.");

        var passwordHash = string.Empty;
        try
        {
            passwordHash = await _dbContext.Customers
                .Where(customer => customer.Email == userEmail)
                .Select(customer => customer.PasswordHash)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError($"{ex.Message}");
            throw;
        }

        if (string.IsNullOrEmpty(passwordHash))
        {
            throw new UnauthorizedAccessException("No matching user found for the supplied email.");
        }

        _logger.LogInformation($"{nameof(CustomerService)} - {nameof(GetUserPasswordHashByEmailAsync)} - completed.");

        return passwordHash;
    }
}
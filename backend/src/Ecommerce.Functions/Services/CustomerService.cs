using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Functions;

public class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly AppDbContext _dbContext;

    public CustomerService(ILogger<CustomerService> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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
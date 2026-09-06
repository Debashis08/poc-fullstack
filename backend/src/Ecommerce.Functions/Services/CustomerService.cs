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

    public async Task<string> GetUserPasswordHashByEmailAsync(string userEmail)
    {
        _logger.LogInformation($"{nameof(CustomerService)} - {nameof(GetUserPasswordHashByEmailAsync)} - started.");
        var passwordHash=await _dbContext.Customers.Where(customer => customer.Email==userEmail).Select(customer => customer.PasswordHash).FirstOrDefaultAsync();
        _logger.LogInformation($"{nameof(CustomerService)} - {nameof(GetUserPasswordHashByEmailAsync)} - completed.");

        return passwordHash!;
    }
}

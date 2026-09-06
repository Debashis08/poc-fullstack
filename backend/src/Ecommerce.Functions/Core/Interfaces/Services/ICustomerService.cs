namespace Ecommerce.Functions;

public interface ICustomerService
{
    public Task<string> GetUserPasswordHashByEmailAsync(string userEmail);
}

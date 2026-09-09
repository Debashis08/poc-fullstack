namespace Ecommerce.Functions;

public interface ICustomerService
{
    public Task<TokenResponse> CreateCustomerProfile(CustomerSignUpRequest customer, CancellationToken cancellationToken = default);

    public Task<string> GetUserPasswordHashByEmailAsync(string userEmail, CancellationToken cancellationToken = default);
}

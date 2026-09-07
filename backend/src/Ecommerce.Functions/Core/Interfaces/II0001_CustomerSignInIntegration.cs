namespace Ecommerce.Functions;

public interface II0001_CustomerSignInIntegration
{
    public Task<string> ProcessCustomerSignIn(CustomerSignInRequest customer, CancellationToken cancellationToken = default);
}

namespace Ecommerce.Functions;

public interface II0001_2_CustomerSignInIntegration
{
    public Task<TokenResponse> ProcessCustomerSignIn(CustomerSignInRequest customer, CancellationToken cancellationToken = default);
}

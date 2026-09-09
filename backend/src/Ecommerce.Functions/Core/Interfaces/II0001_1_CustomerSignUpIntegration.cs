using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public interface II0001_1_CustomerSignUpIntegration
{
    public Task<TokenResponse> ProcessCustomerSignUp(CustomerSignUpRequest customer, CancellationToken cancellationToken = default);
}

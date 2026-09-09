using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public interface ITokenService
{
    public TokenResponse GenerateTokens(CustomerRequest customer);
    public string HashRefreshToken(string refreshToken);
}

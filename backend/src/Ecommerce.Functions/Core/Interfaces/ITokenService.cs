using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public interface ITokenService
{
    public string GenerateToken(Customer customer);
}

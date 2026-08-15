using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class JwtOptions
{
    public string? Key { get; set; } = default!;
    public string? Issuer { get; set; } = default!;
    public string? Audience { get; set; } = default!;
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class JwtOptions
{
    public string? Key { get; set; } = default!;
    public string? Issuer { get; set; } = default!;
    public string? Audience { get; set; } = default!;
    public int AccessTokenExpirationMinutes { get; set; } = 1;
    public int RefreshTokenExpirationDays { get; set; } = 2;
}

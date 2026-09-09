using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class SqlDbOptions
{
    public string? SqlDbCustomerTableName { get; set; } = default!;
    public string? SqlDbCustomerRefreshTokenTableName { get; set; } = default!;
}
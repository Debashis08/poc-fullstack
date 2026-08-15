using System.Text.Json.Serialization;

namespace Ecommerce.Functions;

public class Customer
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("passwordHash")]
    public string? PasswordHash { get; set; }
}
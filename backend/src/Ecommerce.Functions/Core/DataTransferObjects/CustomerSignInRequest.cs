using System.Text.Json.Serialization;

namespace Ecommerce.Functions;
public record CustomerSignInRequest
{
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; init; } = string.Empty;
}

using System.Text.Json.Serialization;

namespace Ecommerce.Functions;
public record CustomerSignUpRequest : CustomerRequest
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; } = default!;

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; } = default!;
}

namespace Ecommerce.Functions;

public class Customer
{
    public long CustomerId { get; set; } = default!;

    public string? Email { get; set; } = default!;

    public string? PasswordHash { get; set; } = default!;

    public string? FirstName { get; set; } = default!;

    public string? LastName { get; set; } = default!;

    public string? PhoneNumber { get; set; } = default!;

    public bool IsActive { get; set; } = default!;

    public DateTime? CreatedAt { get; set; } = default!;

    public DateTime? UpdatedAt { get; set; } = default!;
}
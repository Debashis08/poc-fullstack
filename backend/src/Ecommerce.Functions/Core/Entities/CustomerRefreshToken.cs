using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Functions;

[Table("tbl_customer_refresh_tokens", Schema = "dbo")]
public class CustomerRefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RefreshTokenId { get; set; }

    [Required]
    public long CustomerId { get; set; }

    [Required]
    [MaxLength(500)]
    public string? TokenHash { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }
}

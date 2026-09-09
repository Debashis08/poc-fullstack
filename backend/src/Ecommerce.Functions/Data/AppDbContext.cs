using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecommerce.Functions;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    private readonly SqlDbOptions _sqlDbSettings;

    public AppDbContext(ILogger<AppDbContext> logger, IOptions<SqlDbOptions> sqlDbSettings, DbContextOptions<AppDbContext> options) : base(options)
    {
        _logger = logger;
        _sqlDbSettings = sqlDbSettings.Value ?? throw new ArgumentNullException(nameof(sqlDbSettings));
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerRefreshToken> CustomerRefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation($"{nameof(AppDbContext)} - {nameof(OnModelCreating)} - started.");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.CustomerId);
            entity.ToTable(_sqlDbSettings.SqlDbCustomerTableName);
        });

        modelBuilder.Entity<CustomerRefreshToken>(entity =>
        {
            entity.HasKey(key => key.RefreshTokenId);
            entity.ToTable(_sqlDbSettings.SqlDbCustomerRefreshTokenTableName);

            // Enforce Unique Constraint for Single Device Login
            entity.HasIndex(e => e.CustomerId)
                  .IsUnique()
                  .HasDatabaseName("UQ_tbl_customer_refresh_tokens_CustomerId");

            // Set Default Value for CreatedAt
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("SYSUTCDATETIME()");
        });


        _logger.LogInformation($"{nameof(AppDbContext)} - {nameof(OnModelCreating)} - completed.");
    }
}

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation($"{nameof(AppDbContext)} - {nameof(OnModelCreating)} - started.");
        _logger.LogInformation($"customer sqldb table name {_sqlDbSettings.SqlDbCustomerTableName}");
        modelBuilder.Entity<Customer>().ToTable(_sqlDbSettings.SqlDbCustomerTableName);
        _logger.LogInformation($"{nameof(AppDbContext)} - {nameof(OnModelCreating)} - completed.");
    }
}

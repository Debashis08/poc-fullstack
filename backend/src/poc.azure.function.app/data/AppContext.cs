using Microsoft.EntityFrameworkCore;

namespace poc.azure.function;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable(Environment.GetEnvironmentVariable("SqlTableName"));
    }
}
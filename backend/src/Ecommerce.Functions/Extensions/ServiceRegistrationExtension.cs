using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Functions.Extensions;
public static class ServiceRegistrationExtension
{
    public static IServiceCollection RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<SqlDbOptions>(configuration.GetSection(nameof(SqlDbOptions)));

        return services;
    }

    public static IServiceCollection RegisterIntegrationsAndServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Integrations.
        services.AddScoped<II0001_CustomerLoginIntegration, I0001_CustomerLoginIntegration>();

        // Services.
        services.AddDbContext<AppDbContext>(options =>
        {
            var sqlDbConnectionString = configuration.GetSection("SqlConnectionString").Value;
            options.UseSqlServer(sqlDbConnectionString);
        });
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

        return services;
    }
}

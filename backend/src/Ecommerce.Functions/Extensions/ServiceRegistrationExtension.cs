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
        services.AddScoped<II0001_1_CustomerSignUpIntegration, I0001_1_CustomerSignUpIntegration>();
        services.AddScoped<II0001_2_CustomerSignInIntegration, I0001_2_CustomerSignInIntegration>();

        // Services.
        services.AddDbContext<AppDbContext>(options =>
        {
            var sqlDbConnectionString = configuration.GetSection("SqlConnectionString").Value;
            options.UseSqlServer(sqlDbConnectionString, sql =>
            {
                sql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                sql.CommandTimeout(30);
            });
        });
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPasswordHasher<CustomerSignInRequest>, PasswordHasher<CustomerSignInRequest>>();
        services.AddScoped<IPasswordHasher<CustomerSignUpRequest>, PasswordHasher<CustomerSignUpRequest>>();

        return services;
    }
}

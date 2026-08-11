using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Functions.Extensions;
public static class ServiceRegistrationExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Integrations.
        services.AddScoped<II0001_CustomerLoginIntegration, I0001_CustomerLoginIntegration>();

        // Services.
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        return services;
    }
}

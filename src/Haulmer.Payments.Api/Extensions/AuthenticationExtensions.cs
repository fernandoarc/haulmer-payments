using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haulmer.Payments.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication();

        return services;
    }
}
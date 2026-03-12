using Microsoft.Extensions.DependencyInjection;

namespace Haulmer.Payments.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
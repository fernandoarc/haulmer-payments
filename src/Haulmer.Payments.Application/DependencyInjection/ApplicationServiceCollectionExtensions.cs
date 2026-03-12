using FluentValidation;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Microsoft.Extensions.DependencyInjection;

namespace Haulmer.Payments.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreatePaymentCommandHandler>();
        services.AddValidatorsFromAssembly(typeof(CreatePaymentCommandValidator).Assembly);

        return services;
    }
}
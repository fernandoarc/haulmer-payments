using FluentValidation;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Haulmer.Payments.Application.Payments.Queries.GetPaymentById;
using Haulmer.Payments.Application.Payments.Queries.SearchPayments;
using Microsoft.Extensions.DependencyInjection;

namespace Haulmer.Payments.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreatePaymentCommandHandler>();
        services.AddScoped<GetPaymentByIdQueryHandler>();
        services.AddScoped<SearchPaymentsQueryHandler>();
        services.AddValidatorsFromAssembly(typeof(CreatePaymentCommandValidator).Assembly);

        return services;
    }
}
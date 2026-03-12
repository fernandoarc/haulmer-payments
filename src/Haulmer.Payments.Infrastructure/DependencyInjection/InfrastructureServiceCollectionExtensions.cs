using Haulmer.Payments.Infrastructure.Persistence;
using Haulmer.Payments.Infrastructure.Persistence.Repositories;
using Haulmer.Payments.Infrastructure.ExternalServices.BankAuthorization;
using Haulmer.Payments.Application.Payments.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haulmer.Payments.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PaymentsDb");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Missing required connection string: ConnectionStrings:PaymentsDb");

        services.AddDbContext<PaymentsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IMerchantBranchRepository, MerchantBranchRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IPaymentChannelRepository, PaymentChannelRepository>();
        services.AddScoped<IMerchantPaymentMethodRepository, MerchantPaymentMethodRepository>();
        services.AddScoped<IMerchantAcquirerConfigurationRepository, MerchantAcquirerConfigurationRepository>();
        services.AddScoped<IMerchantPricingRepository, MerchantPricingRepository>();
        services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
        services.AddScoped<IPaymentTransactionStatusHistoryRepository, PaymentTransactionStatusHistoryRepository>();
        services.AddScoped<IPaymentIdempotencyRepository, PaymentIdempotencyRepository>();
        services.AddScoped<IPaymentTraceLogRepository, PaymentTraceLogRepository>();
        services.AddScoped<IPaymentRequestRepository, PaymentRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBankTransactionAuthorizationService, SimulatedBankTransactionAuthorizationService>();

        return services;
    }
}
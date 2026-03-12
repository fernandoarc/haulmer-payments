using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class MerchantAcquirerConfigurationRepository : IMerchantAcquirerConfigurationRepository
{
    private readonly PaymentsDbContext _context;

    public MerchantAcquirerConfigurationRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<MerchantAcquirerConfiguration?> GetActiveByMerchantAndPaymentDetailsAsync(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        string currency,
        CancellationToken cancellationToken)
    {
        return _context.MerchantAcquirerConfigurations
            .AsNoTracking()
            .Where(x =>
                x.MerchantId == merchantId &&
                x.PaymentMethodId == paymentMethodId &&
                x.PaymentChannelId == paymentChannelId &&
                x.Currency == currency.ToUpper() &&
                x.Status == MerchantAcquirerConfigurationStatus.Active)
            .OrderBy(x => x.Priority)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
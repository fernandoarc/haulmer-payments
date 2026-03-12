using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class MerchantPricingRepository : IMerchantPricingRepository
{
    private readonly PaymentsDbContext _context;

    public MerchantPricingRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<MerchantPricing?> GetActiveByMerchantAndPaymentDetailsAsync(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        string currency,
        CancellationToken cancellationToken)
    {
        return _context.MerchantPricings
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.MerchantId == merchantId &&
                x.PaymentMethodId == paymentMethodId &&
                x.PaymentChannelId == paymentChannelId &&
                x.Currency == currency &&
                x.Status == MerchantPricingStatus.Active,
                cancellationToken);
    }
}
using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IMerchantPricingRepository
{
    Task<MerchantPricing?> GetActiveByMerchantAndPaymentDetailsAsync(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        string currency,
        CancellationToken cancellationToken);
}

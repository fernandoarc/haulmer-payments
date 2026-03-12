using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IMerchantAcquirerConfigurationRepository
{
    Task<MerchantAcquirerConfiguration?> GetActiveByMerchantAndPaymentDetailsAsync(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        string currency,
        CancellationToken cancellationToken);
}

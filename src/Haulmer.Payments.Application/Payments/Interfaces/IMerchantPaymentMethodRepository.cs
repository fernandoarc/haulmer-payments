using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IMerchantPaymentMethodRepository
{
    Task<MerchantPaymentMethod> GetByMerchantAndPaymentMethodAsync(
        long merchantId,
        long paymentMethodId,
        CancellationToken cancellationToken);
}

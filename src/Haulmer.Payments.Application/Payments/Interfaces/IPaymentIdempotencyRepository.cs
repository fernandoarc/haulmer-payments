using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentIdempotencyRepository
{
    Task<PaymentIdempotency?> GetByMerchantAndKeyAsync(
        long merchantId,
        string idempotencyKey,
        CancellationToken cancellationToken);

    Task AddAsync(PaymentIdempotency paymentIdempotency, CancellationToken cancellationToken);
}

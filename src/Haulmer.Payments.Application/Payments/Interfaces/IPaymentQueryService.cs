using Haulmer.Payments.Application.Payments.Queries.GetPaymentById;
using Haulmer.Payments.Application.Payments.Queries.SearchPayments;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentQueryService
{
    Task<GetPaymentByIdResult?> GetPaymentByIdAsync(long transactionId, CancellationToken cancellationToken);

    Task<SearchPaymentsResult> SearchPaymentsAsync(
        long merchantId,
        PaymentTransactionStatus? status,
        CancellationToken cancellationToken);
}

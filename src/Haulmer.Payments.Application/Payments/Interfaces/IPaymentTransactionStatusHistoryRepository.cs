using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentTransactionStatusHistoryRepository
{
    Task AddAsync(PaymentTransactionStatusHistory statusHistory, CancellationToken cancellationToken);
}

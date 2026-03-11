using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentRequestRepository
{
    Task AddAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken);
}

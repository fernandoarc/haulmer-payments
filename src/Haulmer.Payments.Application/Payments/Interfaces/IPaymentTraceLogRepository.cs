using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentTraceLogRepository
{
    Task AddAsync(PaymentTraceLog traceLog, CancellationToken cancellationToken);
}

using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentTraceLogRepository : IPaymentTraceLogRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentTraceLogRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentTraceLog traceLog, CancellationToken cancellationToken)
    {
        await _context.PaymentTraceLogs.AddAsync(traceLog, cancellationToken);
    }
}
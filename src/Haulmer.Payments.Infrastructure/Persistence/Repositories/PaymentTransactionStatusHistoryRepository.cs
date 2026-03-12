using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentTransactionStatusHistoryRepository : IPaymentTransactionStatusHistoryRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentTransactionStatusHistoryRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentTransactionStatusHistory history, CancellationToken cancellationToken)
    {
        await _context.PaymentTransactionStatusHistories.AddAsync(history, cancellationToken);
    }
}
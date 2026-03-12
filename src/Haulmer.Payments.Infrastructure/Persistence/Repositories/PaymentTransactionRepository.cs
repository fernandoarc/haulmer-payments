using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentTransactionRepository : IPaymentTransactionRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentTransactionRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentTransaction transaction, CancellationToken cancellationToken)
    {
        await _context.PaymentTransactions.AddAsync(transaction, cancellationToken);
    }
}
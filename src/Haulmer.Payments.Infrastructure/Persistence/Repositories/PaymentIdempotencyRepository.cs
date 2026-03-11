using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentIdempotencyRepository : IPaymentIdempotencyRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentIdempotencyRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<PaymentIdempotency?> GetByMerchantAndKeyAsync(
        long merchantId,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        return _context.PaymentIdempotencies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.MerchantId == merchantId && x.IdempotencyKey == idempotencyKey,
                cancellationToken);
    }

    public async Task AddAsync(PaymentIdempotency idempotency, CancellationToken cancellationToken)
    {
        await _context.PaymentIdempotencies.AddAsync(idempotency, cancellationToken);
    }
}
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentRequestRepository : IPaymentRequestRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentRequestRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        await _context.PaymentRequests.AddAsync(request, cancellationToken);
    }
}
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentMethodRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<PaymentMethod?> GetByIdAsync(long paymentMethodId, CancellationToken cancellationToken)
    {
        return _context.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PaymentMethodId == paymentMethodId, cancellationToken);
    }
}
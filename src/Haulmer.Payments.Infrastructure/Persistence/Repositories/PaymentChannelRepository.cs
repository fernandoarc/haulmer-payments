using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class PaymentChannelRepository : IPaymentChannelRepository
{
    private readonly PaymentsDbContext _context;

    public PaymentChannelRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<PaymentChannel?> GetByIdAsync(long paymentChannelId, CancellationToken cancellationToken)
    {
        return _context.PaymentChannels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PaymentChannelId == paymentChannelId, cancellationToken);
    }
}
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Merchants;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class MerchantRepository : IMerchantRepository
{
    private readonly PaymentsDbContext _context;

    public MerchantRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<Merchant?> GetByIdAsync(long merchantId, CancellationToken cancellationToken)
    {
        return _context.Merchants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MerchantId == merchantId, cancellationToken);
    }
}
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.Merchants;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class MerchantBranchRepository : IMerchantBranchRepository
{
    private readonly PaymentsDbContext _context;

    public MerchantBranchRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<MerchantBranch?> GetByIdAsync(long merchantBranchId, CancellationToken cancellationToken)
    {
        return _context.MerchantBranches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MerchantBranchId == merchantBranchId, cancellationToken);
    }
}
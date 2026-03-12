using Haulmer.Payments.Domain.Merchants;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IMerchantBranchRepository
{
    Task<MerchantBranch?> GetByIdAsync(long merchantBranchId, CancellationToken cancellationToken);
}

using Haulmer.Payments.Domain.Merchants;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IMerchantRepository
{
    Task<Merchant> GetByIdAsync(long merchantId, CancellationToken cancellationToken);
}

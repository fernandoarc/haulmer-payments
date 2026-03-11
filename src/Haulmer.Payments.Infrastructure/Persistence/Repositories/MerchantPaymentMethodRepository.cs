using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Repositories;

public class MerchantPaymentMethodRepository : IMerchantPaymentMethodRepository
{
    private readonly PaymentsDbContext _context;

    public MerchantPaymentMethodRepository(PaymentsDbContext context)
    {
        _context = context;
    }

    public Task<MerchantPaymentMethod?> GetByMerchantAndPaymentMethodAsync(
        long merchantId,
        long paymentMethodId,
        CancellationToken cancellationToken)
    {
        return _context.MerchantPaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.MerchantId == merchantId &&
                x.PaymentMethodId == paymentMethodId &&
                x.Status == MerchantPaymentMethodStatus.Active,
                cancellationToken);
    }
}
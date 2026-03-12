using Haulmer.Payments.Domain.Catalogs;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentMethodRepository
{
    Task<PaymentMethod?> GetByIdAsync(long paymentMethodId, CancellationToken cancellationToken);
}

using Haulmer.Payments.Domain.Catalogs;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IPaymentChannelRepository
{
    Task<PaymentChannel> GetByIdAsync(long paymentChannelId, CancellationToken cancellationToken);
}

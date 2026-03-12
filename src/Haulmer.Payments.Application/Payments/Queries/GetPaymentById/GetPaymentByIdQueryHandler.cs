using Haulmer.Payments.Application.Payments.Interfaces;

namespace Haulmer.Payments.Application.Payments.Queries.GetPaymentById;

public sealed class GetPaymentByIdQueryHandler
{
    private readonly IPaymentQueryService _paymentQueryService;

    public GetPaymentByIdQueryHandler(IPaymentQueryService paymentQueryService)
    {
        _paymentQueryService = paymentQueryService;
    }

    public Task<GetPaymentByIdResult?> HandleAsync(
        GetPaymentByIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.TransactionId <= 0)
            throw new ArgumentException("TransactionId must be greater than zero.", nameof(query.TransactionId));

        return _paymentQueryService.GetPaymentByIdAsync(query.TransactionId, cancellationToken);
    }
}

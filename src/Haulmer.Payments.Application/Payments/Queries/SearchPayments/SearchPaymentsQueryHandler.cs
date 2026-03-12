using Haulmer.Payments.Application.Payments.Interfaces;

namespace Haulmer.Payments.Application.Payments.Queries.SearchPayments;

public sealed class SearchPaymentsQueryHandler
{
    private readonly IPaymentQueryService _paymentQueryService;

    public SearchPaymentsQueryHandler(IPaymentQueryService paymentQueryService)
    {
        _paymentQueryService = paymentQueryService;
    }

    public Task<SearchPaymentsResult> HandleAsync(
        SearchPaymentsQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.MerchantId <= 0)
            throw new ArgumentException("MerchantId must be greater than zero.", nameof(query.MerchantId));

        return _paymentQueryService.SearchPaymentsAsync(
            query.MerchantId,
            query.Status,
            cancellationToken);
    }
}

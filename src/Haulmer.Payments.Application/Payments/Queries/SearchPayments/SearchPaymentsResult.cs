namespace Haulmer.Payments.Application.Payments.Queries.SearchPayments;

public sealed class SearchPaymentsResult
{
    public IReadOnlyCollection<PaymentSummaryResult> Items { get; set; } = Array.Empty<PaymentSummaryResult>();
}

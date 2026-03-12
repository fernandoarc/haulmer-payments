namespace Haulmer.Payments.Api.Features.Payments.SearchPayments;

public sealed record SearchPaymentsResponse
{
	public IReadOnlyCollection<PaymentSummaryResponse> Items { get; init; } = Array.Empty<PaymentSummaryResponse>();
}

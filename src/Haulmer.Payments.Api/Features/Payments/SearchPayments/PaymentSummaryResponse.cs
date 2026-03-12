namespace Haulmer.Payments.Api.Features.Payments.SearchPayments;

public sealed record PaymentSummaryResponse
{
	public long PaymentTransactionId { get; init; }
	public string TransactionNumber { get; init; } = string.Empty;
	public long MerchantId { get; init; }
	public decimal GrossAmount { get; init; }
	public decimal NetAmount { get; init; }
	public string Currency { get; init; } = string.Empty;
	public string Status { get; init; } = string.Empty;
	public string AcquirerReference { get; init; } = string.Empty;
	public Guid CorrelationId { get; init; }
	public DateTime CreatedAtUtc { get; init; }
	public DateTime? ProcessedAtUtc { get; init; }
}

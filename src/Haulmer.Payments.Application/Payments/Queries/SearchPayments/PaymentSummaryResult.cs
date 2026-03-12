namespace Haulmer.Payments.Application.Payments.Queries.SearchPayments;

public sealed class PaymentSummaryResult
{
    public long PaymentTransactionId { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public long MerchantId { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string AcquirerReference { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
}

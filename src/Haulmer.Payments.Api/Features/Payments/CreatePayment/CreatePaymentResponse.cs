namespace Haulmer.Payments.Api.Features.Payments.CreatePayment;

public sealed record CreatePaymentResponse
{
    public long PaymentTransactionId { get; init; }
    public string TransactionNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal GrossAmount { get; init; }
    public decimal NetAmount { get; init; }
    public string AcquirerReference { get; init; } = string.Empty;
    public Guid CorrelationId { get; init; }
}
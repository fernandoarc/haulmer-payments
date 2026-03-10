namespace Haulmer.Payments.Domain.Payments;

public class PaymentTransactionStatusHistory
{
    public long PaymentTransactionStatusHistoryId { get; private set; }
    public long PaymentTransactionId { get; private set; }
    public PaymentTransactionStatus Status { get; private set; }
    public string StatusReasonCode { get; private set; } = string.Empty;
    public string StatusReasonDetail { get; private set; } = string.Empty;
    public string AcquirerResponseCode { get; private set; } = string.Empty;
    public string AcquirerResponseMessage { get; private set; } = string.Empty;
    public Guid CorrelationId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }

    private PaymentTransactionStatusHistory()
    {
    }

    public static PaymentTransactionStatusHistory Create(
        long paymentTransactionId,
        PaymentTransactionStatus status,
        string statusReasonCode,
        string statusReasonDetail,
        string acquirerResponseCode,
        string acquirerResponseMessage,
        Guid correlationId,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentTransactionStatusHistory
        {
            PaymentTransactionId = ValidatePaymentTransactionId(paymentTransactionId),
            Status = status,
            StatusReasonCode = statusReasonCode?.Trim() ?? string.Empty,
            StatusReasonDetail = statusReasonDetail?.Trim() ?? string.Empty,
            AcquirerResponseCode = acquirerResponseCode?.Trim() ?? string.Empty,
            AcquirerResponseMessage = acquirerResponseMessage?.Trim() ?? string.Empty,
            CorrelationId = ValidateCorrelationId(correlationId),
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    private static long ValidatePaymentTransactionId(long paymentTransactionId)
    {
        if (paymentTransactionId <= 0)
            throw new ArgumentException("PaymentTransactionId must be greater than zero.", nameof(paymentTransactionId));

        return paymentTransactionId;
    }

    private static Guid ValidateCorrelationId(Guid correlationId)
    {
        if (correlationId == Guid.Empty)
            throw new ArgumentException("CorrelationId must not be empty.", nameof(correlationId));

        return correlationId;
    }

    private static void ValidateCreationDates(DateTime createdAtUtc, DateTime createdAtLocal)
    {
        if (createdAtUtc == default)
            throw new ArgumentException("createdAtUtc is required.", nameof(createdAtUtc));

        if (createdAtLocal == default)
            throw new ArgumentException("createdAtLocal is required.", nameof(createdAtLocal));
    }
}
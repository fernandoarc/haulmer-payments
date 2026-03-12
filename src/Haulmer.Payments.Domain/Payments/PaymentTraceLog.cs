namespace Haulmer.Payments.Domain.Payments;

public class PaymentTraceLog
{
    public long PaymentTraceLogId { get; private set; }
    public long? PaymentTransactionId { get; private set; }
    public long? MerchantId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string EventSource { get; private set; } = string.Empty;
    public string EventDescription { get; private set; } = string.Empty;
    public string EventDataJson { get; private set; } = string.Empty;
    public TraceSeverity Severity { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }

    private PaymentTraceLog()
    {
    }

    public static PaymentTraceLog Create(
        long? paymentTransactionId,
        long? merchantId,
        Guid correlationId,
        string eventType,
        string eventSource,
        string eventDescription,
        string eventDataJson,
        TraceSeverity severity,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentTraceLog
        {
            PaymentTransactionId = paymentTransactionId,
            MerchantId = merchantId,
            CorrelationId = ValidateCorrelationId(correlationId),
            EventType = ValidateRequired(eventType, nameof(eventType)),
            EventSource = ValidateRequired(eventSource, nameof(eventSource)),
            EventDescription = ValidateRequired(eventDescription, nameof(eventDescription)),
            EventDataJson = eventDataJson?.Trim() ?? string.Empty,
            Severity = severity,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
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
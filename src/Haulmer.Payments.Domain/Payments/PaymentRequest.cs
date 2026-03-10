namespace Haulmer.Payments.Domain.Payments;

public class PaymentRequest
{
    public long PaymentRequestId { get; private set; }
    public long PaymentTransactionId { get; private set; }
    public long MerchantId { get; private set; }
    public string RequestPayloadJson { get; private set; } = string.Empty;
    public string RequestHash { get; private set; } = string.Empty;
    public Guid CorrelationId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }

    private PaymentRequest()
    {
    }

    public static PaymentRequest Create(
        long paymentTransactionId,
        long merchantId,
        string requestPayloadJson,
        string requestHash,
        Guid correlationId,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentRequest
        {
            PaymentTransactionId = ValidatePaymentTransactionId(paymentTransactionId),
            MerchantId = ValidateMerchantId(merchantId),
            RequestPayloadJson = ValidateRequired(requestPayloadJson, nameof(requestPayloadJson)),
            RequestHash = ValidateRequired(requestHash, nameof(requestHash)),
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

    private static long ValidateMerchantId(long merchantId)
    {
        if (merchantId <= 0)
            throw new ArgumentException("MerchantId must be greater than zero.", nameof(merchantId));

        return merchantId;
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
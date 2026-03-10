namespace Haulmer.Payments.Domain.Payments;

public class PaymentIdempotency
{
    public long PaymentIdempotencyId { get; private set; }
    public long MerchantId { get; private set; }
    public string IdempotencyKey { get; private set; } = string.Empty;
    public string RequestHash { get; private set; } = string.Empty;
    public long? PaymentTransactionId { get; private set; }
    public PaymentIdempotencyStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? ExpiresAtUtc { get; private set; }
    public DateTime? ExpiresAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private PaymentIdempotency()
    {
    }

    public static PaymentIdempotency Create(
        long merchantId,
        string idempotencyKey,
        string requestHash,
        DateTime createdAtUtc,
        DateTime createdAtLocal,
        DateTime? expiresAtUtc,
        DateTime? expiresAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentIdempotency
        {
            MerchantId = ValidateMerchantId(merchantId),
            IdempotencyKey = ValidateRequired(idempotencyKey, nameof(idempotencyKey)),
            RequestHash = ValidateRequired(requestHash, nameof(requestHash)),
            Status = PaymentIdempotencyStatus.InProgress,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal,
            ExpiresAtUtc = expiresAtUtc,
            ExpiresAtLocal = expiresAtLocal
        };
    }

    public void AssignTransaction(long paymentTransactionId, DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        PaymentTransactionId = paymentTransactionId;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsCompleted(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status != PaymentIdempotencyStatus.InProgress)
            throw new InvalidOperationException("Cannot mark as completed from current status.");

        Status = PaymentIdempotencyStatus.Completed;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsFailed(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status != PaymentIdempotencyStatus.InProgress)
            throw new InvalidOperationException("Cannot mark as failed from current status.");

        Status = PaymentIdempotencyStatus.Failed;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsExpired(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status != PaymentIdempotencyStatus.InProgress)
            throw new InvalidOperationException("Cannot mark as expired from current status.");

        Status = PaymentIdempotencyStatus.Expired;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    private void MarkAsUpdated(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        UpdatedAtUtc = updatedAtUtc;
        UpdatedAtLocal = updatedAtLocal;
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

    private static void ValidateCreationDates(DateTime createdAtUtc, DateTime createdAtLocal)
    {
        if (createdAtUtc == default)
            throw new ArgumentException("createdAtUtc is required.", nameof(createdAtUtc));

        if (createdAtLocal == default)
            throw new ArgumentException("createdAtLocal is required.", nameof(createdAtLocal));
    }

    private static void ValidateUpdateDates(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        if (updatedAtUtc == default)
            throw new ArgumentException("updatedAtUtc is required.", nameof(updatedAtUtc));

        if (updatedAtLocal == default)
            throw new ArgumentException("updatedAtLocal is required.", nameof(updatedAtLocal));
    }
}
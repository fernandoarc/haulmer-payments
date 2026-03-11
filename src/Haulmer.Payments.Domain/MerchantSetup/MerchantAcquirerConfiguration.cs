namespace Haulmer.Payments.Domain.MerchantSetup;

public class MerchantAcquirerConfiguration
{
    public long MerchantAcquirerConfigurationId { get; private set; }
    public long MerchantId { get; private set; }
    public long PaymentMethodId { get; private set; }
    public long PaymentChannelId { get; private set; }
    public long AcquirerId { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public string MerchantTerminalCode { get; private set; } = string.Empty;
    public string AcquirerMerchantCode { get; private set; } = string.Empty;
    public int TimeoutSeconds { get; private set; }
    public int Priority { get; private set; }
    public MerchantAcquirerConfigurationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private MerchantAcquirerConfiguration()
    {
    }

    public static MerchantAcquirerConfiguration Create(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        long acquirerId,
        string currency,
        string merchantTerminalCode,
        string acquirerMerchantCode,
        int timeoutSeconds,
        int priority,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new MerchantAcquirerConfiguration
        {
            MerchantId = ValidateMerchantId(merchantId),
            PaymentMethodId = ValidatePaymentMethodId(paymentMethodId),
            PaymentChannelId = ValidatePaymentChannelId(paymentChannelId),
            AcquirerId = ValidateAcquirerId(acquirerId),
            Currency = ValidateRequired(currency, nameof(currency)),
            MerchantTerminalCode = ValidateRequired(merchantTerminalCode, nameof(merchantTerminalCode)),
            AcquirerMerchantCode = ValidateRequired(acquirerMerchantCode, nameof(acquirerMerchantCode)),
            TimeoutSeconds = ValidateTimeoutSeconds(timeoutSeconds),
            Priority = ValidatePriority(priority),
            Status = MerchantAcquirerConfigurationStatus.Active,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantAcquirerConfigurationStatus.Active)
            return;

        Status = MerchantAcquirerConfigurationStatus.Active;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantAcquirerConfigurationStatus.Inactive)
            return;

        Status = MerchantAcquirerConfigurationStatus.Inactive;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void UpdateRoutingData(
        string merchantTerminalCode,
        string acquirerMerchantCode,
        int timeoutSeconds,
        int priority,
        DateTime updatedAtUtc,
        DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        MerchantTerminalCode = ValidateRequired(merchantTerminalCode, nameof(merchantTerminalCode));
        AcquirerMerchantCode = ValidateRequired(acquirerMerchantCode, nameof(acquirerMerchantCode));
        TimeoutSeconds = ValidateTimeoutSeconds(timeoutSeconds);
        Priority = ValidatePriority(priority);

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

    private static long ValidatePaymentMethodId(long paymentMethodId)
    {
        if (paymentMethodId <= 0)
            throw new ArgumentException("PaymentMethodId must be greater than zero.", nameof(paymentMethodId));

        return paymentMethodId;
    }

    private static long ValidatePaymentChannelId(long paymentChannelId)
    {
        if (paymentChannelId <= 0)
            throw new ArgumentException("PaymentChannelId must be greater than zero.", nameof(paymentChannelId));

        return paymentChannelId;
    }

    private static long ValidateAcquirerId(long acquirerId)
    {
        if (acquirerId <= 0)
            throw new ArgumentException("AcquirerId must be greater than zero.", nameof(acquirerId));

        return acquirerId;
    }

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
    }

    private static int ValidateTimeoutSeconds(int timeoutSeconds)
    {
        if (timeoutSeconds <= 0)
            throw new ArgumentException("TimeoutSeconds must be greater than zero.", nameof(timeoutSeconds));

        return timeoutSeconds;
    }

    private static int ValidatePriority(int priority)
    {
        if (priority <= 0)
            throw new ArgumentException("Priority must be greater than zero.", nameof(priority));

        return priority;
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
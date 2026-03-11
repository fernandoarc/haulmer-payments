namespace Haulmer.Payments.Domain.MerchantSetup;

public class MerchantPaymentMethod
{
    public long MerchantPaymentMethodId { get; private set; }
    public long MerchantId { get; private set; }
    public long PaymentMethodId { get; private set; }
    public MerchantPaymentMethodStatus Status { get; private set; }
    public DateTime EnabledFromUtc { get; private set; }
    public DateTime EnabledFromLocal { get; private set; }
    public DateTime? EnabledToUtc { get; private set; }
    public DateTime? EnabledToLocal { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private MerchantPaymentMethod()
    {
    }

    public static MerchantPaymentMethod Create(
        long merchantId,
        long paymentMethodId,
        DateTime enabledFromUtc,
        DateTime enabledFromLocal,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateEnabledFromDates(enabledFromUtc, enabledFromLocal);
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new MerchantPaymentMethod
        {
            MerchantId = ValidateMerchantId(merchantId),
            PaymentMethodId = ValidatePaymentMethodId(paymentMethodId),
            Status = MerchantPaymentMethodStatus.Active,
            EnabledFromUtc = enabledFromUtc,
            EnabledFromLocal = enabledFromLocal,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantPaymentMethodStatus.Active)
            return;

        Status = MerchantPaymentMethodStatus.Active;
        EnabledToUtc = null;
        EnabledToLocal = null;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantPaymentMethodStatus.Inactive)
            return;

        Status = MerchantPaymentMethodStatus.Inactive;

        if (!EnabledToUtc.HasValue)
            EnabledToUtc = updatedAtUtc;

        if (!EnabledToLocal.HasValue)
            EnabledToLocal = updatedAtLocal;

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

    private static void ValidateEnabledFromDates(DateTime enabledFromUtc, DateTime enabledFromLocal)
    {
        if (enabledFromUtc == default)
            throw new ArgumentException("enabledFromUtc is required.", nameof(enabledFromUtc));

        if (enabledFromLocal == default)
            throw new ArgumentException("enabledFromLocal is required.", nameof(enabledFromLocal));
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
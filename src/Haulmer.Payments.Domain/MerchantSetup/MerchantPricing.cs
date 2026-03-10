namespace Haulmer.Payments.Domain.MerchantSetup;

public class MerchantPricing
{
    public long MerchantPricingId { get; private set; }
    public long MerchantId { get; private set; }
    public long PaymentMethodId { get; private set; }
    public long PaymentChannelId { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal FixedFeeAmount { get; private set; }
    public decimal VariableFeePercentage { get; private set; }
    public decimal VatPercentage { get; private set; }
    public decimal TaxPercentage { get; private set; }
    public decimal OtherChargePercentage { get; private set; }
    public DateTime ValidFromUtc { get; private set; }
    public DateTime ValidFromLocal { get; private set; }
    public DateTime? ValidToUtc { get; private set; }
    public DateTime? ValidToLocal { get; private set; }
    public MerchantPricingStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }

    private MerchantPricing()
    {
    }

    public static MerchantPricing Create(
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        string currency,
        decimal fixedFeeAmount,
        decimal variableFeePercentage,
        decimal vatPercentage,
        decimal taxPercentage,
        decimal otherChargePercentage,
        DateTime validFromUtc,
        DateTime validFromLocal,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateEnabledFromDates(validFromUtc, validFromLocal);
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new MerchantPricing
        {
            MerchantId = ValidateMerchantId(merchantId),
            PaymentMethodId = ValidatePaymentMethodId(paymentMethodId),
            PaymentChannelId = ValidatePaymentChannelId(paymentChannelId),
            Currency = ValidateRequired(currency, nameof(currency)),
            FixedFeeAmount = ValidateFixedFeeAmount(fixedFeeAmount),
            VariableFeePercentage = ValidateVariableFeePercentage(variableFeePercentage),
            VatPercentage = ValidateVatPercentage(vatPercentage),
            TaxPercentage = ValidateTaxPercentage(taxPercentage),
            OtherChargePercentage = ValidateOtherChargePercentage(otherChargePercentage),
            ValidFromUtc = validFromUtc,
            ValidFromLocal = validFromLocal,
            Status = MerchantPricingStatus.Active,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void Activate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantPricingStatus.Active)
            return;

        Status = MerchantPricingStatus.Active;
        ValidToUtc = null;
        ValidToLocal = null;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void Deactivate(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (Status == MerchantPricingStatus.Inactive)
            return;

        Status = MerchantPricingStatus.Inactive;
        if (!ValidToUtc.HasValue)
            ValidToUtc = updatedAtUtc;
        if (!ValidToLocal.HasValue)
            ValidToLocal = updatedAtLocal;
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

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
    }

    private static decimal ValidateFixedFeeAmount(decimal fixedFeeAmount)
    {
        if (fixedFeeAmount < 0)
            throw new ArgumentException("FixedFeeAmount must be greater than or equal to zero.", nameof(fixedFeeAmount));

        return fixedFeeAmount;
    }

    private static decimal ValidateVariableFeePercentage(decimal variableFeePercentage)
    {
        if (variableFeePercentage < 0)
            throw new ArgumentException("VariableFeePercentage must be greater than or equal to zero.", nameof(variableFeePercentage));

        return variableFeePercentage;
    }

    private static decimal ValidateVatPercentage(decimal vatPercentage)
    {
        if (vatPercentage < 0)
            throw new ArgumentException("VatPercentage must be greater than or equal to zero.", nameof(vatPercentage));

        return vatPercentage;
    }

    private static decimal ValidateTaxPercentage(decimal taxPercentage)
    {
        if (taxPercentage < 0)
            throw new ArgumentException("TaxPercentage must be greater than or equal to zero.", nameof(taxPercentage));

        return taxPercentage;
    }

    private static decimal ValidateOtherChargePercentage(decimal otherChargePercentage)
    {
        if (otherChargePercentage < 0)
            throw new ArgumentException("OtherChargePercentage must be greater than or equal to zero.", nameof(otherChargePercentage));

        return otherChargePercentage;
    }

    private static void ValidateEnabledFromDates(DateTime validFromUtc, DateTime validFromLocal)
    {
        if (validFromUtc == default)
            throw new ArgumentException("validFromUtc is required.", nameof(validFromUtc));

        if (validFromLocal == default)
            throw new ArgumentException("validFromLocal is required.", nameof(validFromLocal));
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
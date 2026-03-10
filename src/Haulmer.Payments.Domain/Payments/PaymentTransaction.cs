namespace Haulmer.Payments.Domain.Payments;

public class PaymentTransaction
{
    public long PaymentTransactionId { get; private set; }
    public string TransactionNumber { get; private set; } = string.Empty;
    public long MerchantId { get; private set; }
    public long MerchantBranchId { get; private set; }
    public long PaymentMethodId { get; private set; }
    public long PaymentChannelId { get; private set; }
    public long MerchantPaymentMethodId { get; private set; }
    public long MerchantAcquirerConfigurationId { get; private set; }
    public long AcquirerId { get; private set; }
    public string IdempotencyKey { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public decimal BaseAmount { get; private set; }
    public decimal TipAmount { get; private set; }
    public decimal GrossAmount { get; private set; }
    public decimal FeeAmount { get; private set; }
    public decimal VatAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal OtherChargesAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public string PayerFullName { get; private set; } = string.Empty;
    public string PayerRut { get; private set; } = string.Empty;
    public string IssuingBankName { get; private set; } = string.Empty;
    public string CardBrand { get; private set; } = string.Empty;
    public string CardLast4 { get; private set; } = string.Empty;
    public string MaskedPan { get; private set; } = string.Empty;
    public PaymentTransactionStatus CurrentStatus { get; private set; }
    public string AcquirerReference { get; private set; } = string.Empty;
    public Guid CorrelationId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime CreatedAtLocal { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? UpdatedAtLocal { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }
    public DateTime? ProcessedAtLocal { get; private set; }

    private PaymentTransaction()
    {
    }

    public static PaymentTransaction Create(
        string transactionNumber,
        long merchantId,
        long merchantBranchId,
        long paymentMethodId,
        long paymentChannelId,
        long merchantPaymentMethodId,
        long merchantAcquirerConfigurationId,
        long acquirerId,
        string idempotencyKey,
        string currency,
        decimal baseAmount,
        decimal tipAmount,
        decimal grossAmount,
        decimal feeAmount,
        decimal vatAmount,
        decimal taxAmount,
        decimal otherChargesAmount,
        decimal netAmount,
        string payerFullName,
        string payerRut,
        string issuingBankName,
        string cardBrand,
        string cardLast4,
        string maskedPan,
        Guid correlationId,
        DateTime createdAtUtc,
        DateTime createdAtLocal)
    {
        ValidateCreationDates(createdAtUtc, createdAtLocal);

        return new PaymentTransaction
        {
            TransactionNumber = ValidateRequired(transactionNumber, nameof(transactionNumber)),
            MerchantId = ValidateMerchantId(merchantId),
            MerchantBranchId = ValidateMerchantBranchId(merchantBranchId),
            PaymentMethodId = ValidatePaymentMethodId(paymentMethodId),
            PaymentChannelId = ValidatePaymentChannelId(paymentChannelId),
            MerchantPaymentMethodId = ValidateMerchantPaymentMethodId(merchantPaymentMethodId),
            MerchantAcquirerConfigurationId = ValidateMerchantAcquirerConfigurationId(merchantAcquirerConfigurationId),
            AcquirerId = ValidateAcquirerId(acquirerId),
            IdempotencyKey = ValidateRequired(idempotencyKey, nameof(idempotencyKey)),
            Currency = ValidateRequired(currency, nameof(currency)),
            BaseAmount = ValidateBaseAmount(baseAmount),
            TipAmount = ValidateTipAmount(tipAmount),
            GrossAmount = ValidateGrossAmount(grossAmount, baseAmount, tipAmount),
            FeeAmount = ValidateFeeAmount(feeAmount),
            VatAmount = ValidateVatAmount(vatAmount),
            TaxAmount = ValidateTaxAmount(taxAmount),
            OtherChargesAmount = ValidateOtherChargesAmount(otherChargesAmount),
            NetAmount = ValidateNetAmount(netAmount),
            PayerFullName = ValidateRequired(payerFullName, nameof(payerFullName)),
            PayerRut = ValidateRequired(payerRut, nameof(payerRut)),
            IssuingBankName = ValidateRequired(issuingBankName, nameof(issuingBankName)),
            CardBrand = ValidateRequired(cardBrand, nameof(cardBrand)),
            CardLast4 = ValidateRequired(cardLast4, nameof(cardLast4)),
            MaskedPan = ValidateRequired(maskedPan, nameof(maskedPan)),
            CorrelationId = ValidateCorrelationId(correlationId),
            CurrentStatus = PaymentTransactionStatus.Pending,
            CreatedAtUtc = createdAtUtc,
            CreatedAtLocal = createdAtLocal
        };
    }

    public void MarkAsProcessing(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (CurrentStatus != PaymentTransactionStatus.Pending)
            throw new InvalidOperationException("Cannot mark as processing from current status.");

        CurrentStatus = PaymentTransactionStatus.Processing;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsApproved(string acquirerReference, DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (CurrentStatus != PaymentTransactionStatus.Processing)
            throw new InvalidOperationException("Cannot mark as approved from current status.");

        CurrentStatus = PaymentTransactionStatus.Approved;
        AcquirerReference = acquirerReference ?? string.Empty;
        ProcessedAtUtc = updatedAtUtc;
        ProcessedAtLocal = updatedAtLocal;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsDeclined(string acquirerReference, DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (CurrentStatus != PaymentTransactionStatus.Processing)
            throw new InvalidOperationException("Cannot mark as declined from current status.");

        CurrentStatus = PaymentTransactionStatus.Declined;
        AcquirerReference = acquirerReference ?? string.Empty;
        ProcessedAtUtc = updatedAtUtc;
        ProcessedAtLocal = updatedAtLocal;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    public void MarkAsFailed(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        ValidateUpdateDates(updatedAtUtc, updatedAtLocal);

        if (CurrentStatus != PaymentTransactionStatus.Processing)
            throw new InvalidOperationException("Cannot mark as failed from current status.");

        CurrentStatus = PaymentTransactionStatus.Failed;
        ProcessedAtUtc = updatedAtUtc;
        ProcessedAtLocal = updatedAtLocal;
        MarkAsUpdated(updatedAtUtc, updatedAtLocal);
    }

    private void MarkAsUpdated(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        UpdatedAtUtc = updatedAtUtc;
        UpdatedAtLocal = updatedAtLocal;
    }

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
    }

    private static long ValidateMerchantId(long merchantId)
    {
        if (merchantId <= 0)
            throw new ArgumentException("MerchantId must be greater than zero.", nameof(merchantId));

        return merchantId;
    }

    private static long ValidateMerchantBranchId(long merchantBranchId)
    {
        if (merchantBranchId <= 0)
            throw new ArgumentException("MerchantBranchId must be greater than zero.", nameof(merchantBranchId));

        return merchantBranchId;
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

    private static long ValidateMerchantPaymentMethodId(long merchantPaymentMethodId)
    {
        if (merchantPaymentMethodId <= 0)
            throw new ArgumentException("MerchantPaymentMethodId must be greater than zero.", nameof(merchantPaymentMethodId));

        return merchantPaymentMethodId;
    }

    private static long ValidateMerchantAcquirerConfigurationId(long merchantAcquirerConfigurationId)
    {
        if (merchantAcquirerConfigurationId <= 0)
            throw new ArgumentException("MerchantAcquirerConfigurationId must be greater than zero.", nameof(merchantAcquirerConfigurationId));

        return merchantAcquirerConfigurationId;
    }

    private static long ValidateAcquirerId(long acquirerId)
    {
        if (acquirerId <= 0)
            throw new ArgumentException("AcquirerId must be greater than zero.", nameof(acquirerId));

        return acquirerId;
    }

    private static decimal ValidateBaseAmount(decimal baseAmount)
    {
        if (baseAmount <= 0)
            throw new ArgumentException("BaseAmount must be greater than zero.", nameof(baseAmount));

        return baseAmount;
    }

    private static decimal ValidateTipAmount(decimal tipAmount)
    {
        if (tipAmount < 0)
            throw new ArgumentException("TipAmount must be greater than or equal to zero.", nameof(tipAmount));

        return tipAmount;
    }

    private static decimal ValidateGrossAmount(decimal grossAmount, decimal baseAmount, decimal tipAmount)
    {
        if (grossAmount != baseAmount + tipAmount)
            throw new ArgumentException("GrossAmount must equal BaseAmount + TipAmount.", nameof(grossAmount));

        return grossAmount;
    }

    private static decimal ValidateFeeAmount(decimal feeAmount)
    {
        if (feeAmount < 0)
            throw new ArgumentException("FeeAmount must be greater than or equal to zero.", nameof(feeAmount));

        return feeAmount;
    }

    private static decimal ValidateVatAmount(decimal vatAmount)
    {
        if (vatAmount < 0)
            throw new ArgumentException("VatAmount must be greater than or equal to zero.", nameof(vatAmount));

        return vatAmount;
    }

    private static decimal ValidateTaxAmount(decimal taxAmount)
    {
        if (taxAmount < 0)
            throw new ArgumentException("TaxAmount must be greater than or equal to zero.", nameof(taxAmount));

        return taxAmount;
    }

    private static decimal ValidateOtherChargesAmount(decimal otherChargesAmount)
    {
        if (otherChargesAmount < 0)
            throw new ArgumentException("OtherChargesAmount must be greater than or equal to zero.", nameof(otherChargesAmount));

        return otherChargesAmount;
    }

    private static decimal ValidateNetAmount(decimal netAmount)
    {
        if (netAmount < 0)
            throw new ArgumentException("NetAmount must be greater than or equal to zero.", nameof(netAmount));

        return netAmount;
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

    private static void ValidateUpdateDates(DateTime updatedAtUtc, DateTime updatedAtLocal)
    {
        if (updatedAtUtc == default)
            throw new ArgumentException("updatedAtUtc is required.", nameof(updatedAtUtc));

        if (updatedAtLocal == default)
            throw new ArgumentException("updatedAtLocal is required.", nameof(updatedAtLocal));
    }
}
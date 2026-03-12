namespace Haulmer.Payments.Application.Payments.Queries.GetPaymentById;

public sealed class GetPaymentByIdResult
{
    public long PaymentTransactionId { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public long MerchantId { get; set; }
    public long MerchantBranchId { get; set; }
    public long PaymentMethodId { get; set; }
    public long PaymentChannelId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal BaseAmount { get; set; }
    public decimal TipAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal OtherChargesAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string PayerFullName { get; set; } = string.Empty;
    public string PayerRut { get; set; } = string.Empty;
    public string IssuingBankName { get; set; } = string.Empty;
    public string CardBrand { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public string MaskedPan { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string AcquirerReference { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
}

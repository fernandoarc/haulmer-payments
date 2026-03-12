namespace Haulmer.Payments.Api.Features.Payments.GetPaymentById;

public sealed record GetPaymentByIdResponse
{
	public long PaymentTransactionId { get; init; }
	public string TransactionNumber { get; init; } = string.Empty;
	public long MerchantId { get; init; }
	public long MerchantBranchId { get; init; }
	public long PaymentMethodId { get; init; }
	public long PaymentChannelId { get; init; }
	public string Currency { get; init; } = string.Empty;
	public decimal BaseAmount { get; init; }
	public decimal TipAmount { get; init; }
	public decimal GrossAmount { get; init; }
	public decimal FeeAmount { get; init; }
	public decimal VatAmount { get; init; }
	public decimal TaxAmount { get; init; }
	public decimal OtherChargesAmount { get; init; }
	public decimal NetAmount { get; init; }
	public string PayerFullName { get; init; } = string.Empty;
	public string PayerRut { get; init; } = string.Empty;
	public string IssuingBankName { get; init; } = string.Empty;
	public string CardBrand { get; init; } = string.Empty;
	public string CardLast4 { get; init; } = string.Empty;
	public string MaskedPan { get; init; } = string.Empty;
	public string Status { get; init; } = string.Empty;
	public string AcquirerReference { get; init; } = string.Empty;
	public Guid CorrelationId { get; init; }
	public DateTime CreatedAtUtc { get; init; }
	public DateTime? ProcessedAtUtc { get; init; }
}

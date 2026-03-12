namespace Haulmer.Payments.Api.Features.Payments.CreatePayment;

public sealed record CreatePaymentRequest
{
    public long MerchantId { get; init; }
    public long MerchantBranchId { get; init; }
    public long PaymentMethodId { get; init; }
    public long PaymentChannelId { get; init; }
    public string IdempotencyKey { get; init; } = string.Empty;
    public decimal BaseAmount { get; init; }
    public decimal TipAmount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string PayerFullName { get; init; } = string.Empty;
    public string PayerRut { get; init; } = string.Empty;
    public string RequestPayloadJson { get; init; } = string.Empty;
}
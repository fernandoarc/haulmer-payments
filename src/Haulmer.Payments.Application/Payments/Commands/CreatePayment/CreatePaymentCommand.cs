namespace Haulmer.Payments.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommand
{
    public long MerchantId { get; set; }
    public long MerchantBranchId { get; set; }
    public long PaymentMethodId { get; set; }
    public long PaymentChannelId { get; set; }
    public string? IdempotencyKey { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal TipAmount { get; set; }
    public string? Currency { get; set; }
    public string? PayerFullName { get; set; }
    public string? PayerRut { get; set; }
    public string? RequestPayloadJson { get; set; }
    public Guid CorrelationId { get; set; }
}
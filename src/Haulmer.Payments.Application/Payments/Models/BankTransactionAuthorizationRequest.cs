namespace Haulmer.Payments.Application.Payments.Models;

public class BankTransactionAuthorizationRequest
{
    public long PaymentTransactionId { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public long MerchantId { get; set; }
    public long MerchantBranchId { get; set; }
    public long PaymentMethodId { get; set; }
    public long PaymentChannelId { get; set; }
    public long AcquirerId { get; set; }
    public string Currency { get; set; }= string.Empty;
    public decimal GrossAmount { get; set; }
    public string PayerFullName { get; set; } = string.Empty;
    public string PayerRut { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
}

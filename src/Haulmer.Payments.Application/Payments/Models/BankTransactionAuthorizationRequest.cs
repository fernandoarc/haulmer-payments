namespace Haulmer.Payments.Application.Payments.Models;

public class BankTransactionAuthorizationRequest
{
    public long PaymentTransactionId { get; set; }
    public string TransactionNumber { get; set; }
    public long MerchantId { get; set; }
    public long MerchantBranchId { get; set; }
    public long PaymentMethodId { get; set; }
    public long PaymentChannelId { get; set; }
    public long AcquirerId { get; set; }
    public string Currency { get; set; }
    public decimal GrossAmount { get; set; }
    public string PayerFullName { get; set; }
    public string PayerRut { get; set; }
    public Guid CorrelationId { get; set; }
}

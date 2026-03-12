using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Commands.CreatePayment;

public class CreatePaymentResult
{
    public long PaymentTransactionId { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public PaymentTransactionStatus Status { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string AcquirerReference { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
}
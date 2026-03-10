namespace Haulmer.Payments.Domain.Payments;

public enum PaymentTransactionStatus
{
    Pending = 1,
    Processing = 2,
    Approved = 3,
    Declined = 4,
    Failed = 5
}
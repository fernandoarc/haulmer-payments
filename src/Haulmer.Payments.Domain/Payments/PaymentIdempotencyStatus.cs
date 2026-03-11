namespace Haulmer.Payments.Domain.Payments;

public enum PaymentIdempotencyStatus
{
    InProgress = 1,
    Completed = 2,
    Expired = 3,
    Failed = 4
}
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Queries.SearchPayments;

public sealed class SearchPaymentsQuery
{
    public long MerchantId { get; set; }
    public PaymentTransactionStatus? Status { get; set; }
}

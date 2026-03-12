using Haulmer.Payments.Application.Payments.Models;

namespace Haulmer.Payments.Application.Payments.Interfaces;

public interface IBankTransactionAuthorizationService
{
    Task<BankTransactionAuthorizationResult> AuthorizeTransactionAsync(
        BankTransactionAuthorizationRequest request,
        CancellationToken cancellationToken);
}

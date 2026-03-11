using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Application.Payments.Models;

namespace Haulmer.Payments.Infrastructure.ExternalServices.BankAuthorization;

public class SimulatedBankTransactionAuthorizationService : IBankTransactionAuthorizationService
{
    public Task<BankTransactionAuthorizationResult> AuthorizeTransactionAsync(
        BankTransactionAuthorizationRequest request,
        CancellationToken cancellationToken)
    {
        var acquirerReference = $"SIM-{request.TransactionNumber}";

        var result = request.GrossAmount <= 1000000
            ? new BankTransactionAuthorizationResult
            {
                IsApproved = true,
                ResponseCode = "00",
                ResponseMessage = "Approved",
                AcquirerReference = acquirerReference,
                IssuingBankName = "Banco Simulado",
                CardBrand = "VISA",
                CardLast4 = "1234",
                MaskedPan = "****-****-****-1234"
            }
            : new BankTransactionAuthorizationResult
            {
                IsApproved = false,
                ResponseCode = "05",
                ResponseMessage = "Declined",
                AcquirerReference = acquirerReference,
                IssuingBankName = "Banco Simulado",
                CardBrand = "VISA",
                CardLast4 = "1234",
                MaskedPan = "****-****-****-1234"
            };

        return Task.FromResult(result);
    }
}
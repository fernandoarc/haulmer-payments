using Haulmer.Payments.Api.Features.Payments.CreatePayment;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;

namespace Haulmer.Payments.Api.Features.Payments.Mappings;

public static class PaymentsContractsMapper
{
    public static CreatePaymentCommand ToCreatePaymentCommand(
        this CreatePaymentRequest request,
        Guid correlationId)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CreatePaymentCommand
        {
            MerchantId = request.MerchantId,
            MerchantBranchId = request.MerchantBranchId,
            PaymentMethodId = request.PaymentMethodId,
            PaymentChannelId = request.PaymentChannelId,
            IdempotencyKey = request.IdempotencyKey,
            BaseAmount = request.BaseAmount,
            TipAmount = request.TipAmount,
            Currency = request.Currency,
            PayerFullName = request.PayerFullName,
            PayerRut = request.PayerRut,
            RequestPayloadJson = request.RequestPayloadJson,
            CorrelationId = correlationId
        };
    }

    public static CreatePaymentResponse ToCreatePaymentResponse(this CreatePaymentResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new CreatePaymentResponse
        {
            PaymentTransactionId = result.PaymentTransactionId,
            TransactionNumber = result.TransactionNumber,
            Status = result.Status.ToString(),
            GrossAmount = result.GrossAmount,
            NetAmount = result.NetAmount,
            AcquirerReference = result.AcquirerReference,
            CorrelationId = result.CorrelationId
        };
    }
}
using Haulmer.Payments.Api.Features.Payments.CreatePayment;
using Haulmer.Payments.Api.Features.Payments.GetPaymentById;
using Haulmer.Payments.Api.Features.Payments.SearchPayments;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Haulmer.Payments.Application.Payments.Queries.GetPaymentById;
using Haulmer.Payments.Application.Payments.Queries.SearchPayments;

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

    public static GetPaymentByIdResponse ToGetPaymentByIdResponse(this GetPaymentByIdResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new GetPaymentByIdResponse
        {
            PaymentTransactionId = result.PaymentTransactionId,
            TransactionNumber = result.TransactionNumber,
            MerchantId = result.MerchantId,
            MerchantBranchId = result.MerchantBranchId,
            PaymentMethodId = result.PaymentMethodId,
            PaymentChannelId = result.PaymentChannelId,
            Currency = result.Currency,
            BaseAmount = result.BaseAmount,
            TipAmount = result.TipAmount,
            GrossAmount = result.GrossAmount,
            FeeAmount = result.FeeAmount,
            VatAmount = result.VatAmount,
            TaxAmount = result.TaxAmount,
            OtherChargesAmount = result.OtherChargesAmount,
            NetAmount = result.NetAmount,
            PayerFullName = result.PayerFullName,
            PayerRut = result.PayerRut,
            IssuingBankName = result.IssuingBankName,
            CardBrand = result.CardBrand,
            CardLast4 = result.CardLast4,
            MaskedPan = result.MaskedPan,
            Status = result.Status,
            AcquirerReference = result.AcquirerReference,
            CorrelationId = result.CorrelationId,
            CreatedAtUtc = result.CreatedAtUtc,
            ProcessedAtUtc = result.ProcessedAtUtc
        };
    }

    public static PaymentSummaryResponse ToPaymentSummaryResponse(this PaymentSummaryResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new PaymentSummaryResponse
        {
            PaymentTransactionId = result.PaymentTransactionId,
            TransactionNumber = result.TransactionNumber,
            MerchantId = result.MerchantId,
            GrossAmount = result.GrossAmount,
            NetAmount = result.NetAmount,
            Currency = result.Currency,
            Status = result.Status,
            AcquirerReference = result.AcquirerReference,
            CorrelationId = result.CorrelationId,
            CreatedAtUtc = result.CreatedAtUtc,
            ProcessedAtUtc = result.ProcessedAtUtc
        };
    }

    public static SearchPaymentsResponse ToSearchPaymentsResponse(this SearchPaymentsResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new SearchPaymentsResponse
        {
            Items = result.Items
                .Select(x => x.ToPaymentSummaryResponse())
                .ToArray()
        };
    }
}
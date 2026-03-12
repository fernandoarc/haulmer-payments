using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Application.Payments.Queries.GetPaymentById;
using Haulmer.Payments.Application.Payments.Queries.SearchPayments;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence.Queries;

public sealed class PaymentQueryService : IPaymentQueryService
{
    private readonly PaymentsDbContext _dbContext;

    public PaymentQueryService(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetPaymentByIdResult?> GetPaymentByIdAsync(long transactionId, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.PaymentTransactions
            .AsNoTracking()
            .Where(x => x.PaymentTransactionId == transactionId)
            .Select(x => new
            {
                x.PaymentTransactionId,
                x.TransactionNumber,
                x.MerchantId,
                x.MerchantBranchId,
                x.PaymentMethodId,
                x.PaymentChannelId,
                x.Currency,
                x.BaseAmount,
                x.TipAmount,
                x.GrossAmount,
                x.FeeAmount,
                x.VatAmount,
                x.TaxAmount,
                x.OtherChargesAmount,
                x.NetAmount,
                x.PayerFullName,
                x.PayerRut,
                x.IssuingBankName,
                x.CardBrand,
                x.CardLast4,
                x.MaskedPan,
                x.CurrentStatus,
                x.AcquirerReference,
                x.CorrelationId,
                x.CreatedAtUtc,
                x.ProcessedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (transaction is null)
            return null;

        return new GetPaymentByIdResult
        {
            PaymentTransactionId = transaction.PaymentTransactionId,
            TransactionNumber = transaction.TransactionNumber,
            MerchantId = transaction.MerchantId,
            MerchantBranchId = transaction.MerchantBranchId,
            PaymentMethodId = transaction.PaymentMethodId,
            PaymentChannelId = transaction.PaymentChannelId,
            Currency = transaction.Currency,
            BaseAmount = transaction.BaseAmount,
            TipAmount = transaction.TipAmount,
            GrossAmount = transaction.GrossAmount,
            FeeAmount = transaction.FeeAmount,
            VatAmount = transaction.VatAmount,
            TaxAmount = transaction.TaxAmount,
            OtherChargesAmount = transaction.OtherChargesAmount,
            NetAmount = transaction.NetAmount,
            PayerFullName = transaction.PayerFullName,
            PayerRut = transaction.PayerRut,
            IssuingBankName = transaction.IssuingBankName,
            CardBrand = transaction.CardBrand,
            CardLast4 = transaction.CardLast4,
            MaskedPan = transaction.MaskedPan,
            Status = transaction.CurrentStatus.ToString(),
            AcquirerReference = transaction.AcquirerReference,
            CorrelationId = transaction.CorrelationId,
            CreatedAtUtc = transaction.CreatedAtUtc,
            ProcessedAtUtc = transaction.ProcessedAtUtc
        };
    }

    public async Task<SearchPaymentsResult> SearchPaymentsAsync(
        long merchantId,
        PaymentTransactionStatus? status,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.PaymentTransactions
            .AsNoTracking()
            .Where(x => x.MerchantId == merchantId);

        if (status.HasValue)
            query = query.Where(x => x.CurrentStatus == status.Value);

        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new
            {
                x.PaymentTransactionId,
                x.TransactionNumber,
                x.MerchantId,
                x.GrossAmount,
                x.NetAmount,
                x.Currency,
                x.CurrentStatus,
                x.AcquirerReference,
                x.CorrelationId,
                x.CreatedAtUtc,
                x.ProcessedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new SearchPaymentsResult
        {
            Items = items
                .Select(x => new PaymentSummaryResult
                {
                    PaymentTransactionId = x.PaymentTransactionId,
                    TransactionNumber = x.TransactionNumber,
                    MerchantId = x.MerchantId,
                    GrossAmount = x.GrossAmount,
                    NetAmount = x.NetAmount,
                    Currency = x.Currency,
                    Status = x.CurrentStatus.ToString(),
                    AcquirerReference = x.AcquirerReference,
                    CorrelationId = x.CorrelationId,
                    CreatedAtUtc = x.CreatedAtUtc,
                    ProcessedAtUtc = x.ProcessedAtUtc
                })
                .ToArray()
        };
    }
}

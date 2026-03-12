using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Application.Payments.Models;
using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler
{
    private const string PendingIssuingBankName = "PENDING_AUTHORIZATION";
    private const string PendingCardBrand = "PENDING_AUTHORIZATION";
    private const string PendingCardLast4 = "0000";
    private const string PendingMaskedPan = "****-****-****-0000";

    private readonly IMerchantRepository _merchantRepository;
    private readonly IMerchantBranchRepository _merchantBranchRepository;
    private readonly IMerchantPaymentMethodRepository _merchantPaymentMethodRepository;
    private readonly IMerchantAcquirerConfigurationRepository _merchantAcquirerConfigurationRepository;
    private readonly IMerchantPricingRepository _merchantPricingRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IPaymentChannelRepository _paymentChannelRepository;
    private readonly IPaymentTransactionRepository _paymentTransactionRepository;
    private readonly IPaymentTransactionStatusHistoryRepository _paymentTransactionStatusHistoryRepository;
    private readonly IPaymentIdempotencyRepository _paymentIdempotencyRepository;
    private readonly IPaymentTraceLogRepository _paymentTraceLogRepository;
    private readonly IPaymentRequestRepository _paymentRequestRepository;
    private readonly IBankTransactionAuthorizationService _bankAuthorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IMerchantRepository merchantRepository,
        IMerchantBranchRepository merchantBranchRepository,
        IMerchantPaymentMethodRepository merchantPaymentMethodRepository,
        IMerchantAcquirerConfigurationRepository merchantAcquirerConfigurationRepository,
        IMerchantPricingRepository merchantPricingRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IPaymentChannelRepository paymentChannelRepository,
        IPaymentTransactionRepository paymentTransactionRepository,
        IPaymentTransactionStatusHistoryRepository paymentTransactionStatusHistoryRepository,
        IPaymentIdempotencyRepository paymentIdempotencyRepository,
        IPaymentTraceLogRepository paymentTraceLogRepository,
        IPaymentRequestRepository paymentRequestRepository,
        IBankTransactionAuthorizationService bankAuthorizationService,
        IUnitOfWork unitOfWork)
    {
        _merchantRepository = merchantRepository;
        _merchantBranchRepository = merchantBranchRepository;
        _merchantPaymentMethodRepository = merchantPaymentMethodRepository;
        _merchantAcquirerConfigurationRepository = merchantAcquirerConfigurationRepository;
        _merchantPricingRepository = merchantPricingRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _paymentChannelRepository = paymentChannelRepository;
        _paymentTransactionRepository = paymentTransactionRepository;
        _paymentTransactionStatusHistoryRepository = paymentTransactionStatusHistoryRepository;
        _paymentIdempotencyRepository = paymentIdempotencyRepository;
        _paymentTraceLogRepository = paymentTraceLogRepository;
        _paymentRequestRepository = paymentRequestRepository;
        _bankAuthorizationService = bankAuthorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePaymentResult> HandleAsync(
        CreatePaymentCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var now = DateTime.UtcNow;
        var nowLocal = DateTime.Now;

        var idempotencyKey = command.IdempotencyKey?.Trim() ?? string.Empty;
        var currency = command.Currency?.Trim() ?? string.Empty;
        var payerFullName = command.PayerFullName?.Trim() ?? string.Empty;
        var payerRut = command.PayerRut?.Trim() ?? string.Empty;
        var requestPayloadJson = command.RequestPayloadJson ?? string.Empty;

        var merchant = await _merchantRepository.GetByIdAsync(command.MerchantId, cancellationToken);
        if (merchant is null)
            throw new InvalidOperationException($"Merchant {command.MerchantId} not found.");

        if (merchant.Status != MerchantStatus.Active)
            throw new InvalidOperationException($"Merchant {command.MerchantId} is not active.");

        var merchantBranch = await _merchantBranchRepository.GetByIdAsync(command.MerchantBranchId, cancellationToken);
        if (merchantBranch is null)
            throw new InvalidOperationException($"MerchantBranch {command.MerchantBranchId} not found.");

        if (merchantBranch.MerchantId != command.MerchantId)
            throw new InvalidOperationException(
                $"MerchantBranch {command.MerchantBranchId} does not belong to merchant {command.MerchantId}.");

        if (merchantBranch.Status != MerchantBranchStatus.Active)
            throw new InvalidOperationException($"MerchantBranch {command.MerchantBranchId} is not active.");

        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(command.PaymentMethodId, cancellationToken);
        if (paymentMethod is null)
            throw new InvalidOperationException($"PaymentMethod {command.PaymentMethodId} not found.");

        if (paymentMethod.Status != PaymentMethodStatus.Active)
            throw new InvalidOperationException($"PaymentMethod {command.PaymentMethodId} is not active.");

        var paymentChannel = await _paymentChannelRepository.GetByIdAsync(command.PaymentChannelId, cancellationToken);
        if (paymentChannel is null)
            throw new InvalidOperationException($"PaymentChannel {command.PaymentChannelId} not found.");

        if (paymentChannel.Status != PaymentChannelStatus.Active)
            throw new InvalidOperationException($"PaymentChannel {command.PaymentChannelId} is not active.");

        var merchantPaymentMethod = await _merchantPaymentMethodRepository.GetByMerchantAndPaymentMethodAsync(
            command.MerchantId,
            command.PaymentMethodId,
            cancellationToken);

        if (merchantPaymentMethod is null)
        {
            throw new InvalidOperationException(
                $"MerchantPaymentMethod not found for merchant {command.MerchantId} and payment method {command.PaymentMethodId}.");
        }

        if (merchantPaymentMethod.Status != MerchantPaymentMethodStatus.Active)
            throw new InvalidOperationException("MerchantPaymentMethod is not active.");

        var acquirerConfiguration = await _merchantAcquirerConfigurationRepository.GetActiveByMerchantAndPaymentDetailsAsync(
            command.MerchantId,
            command.PaymentMethodId,
            command.PaymentChannelId,
            currency,
            cancellationToken);

        if (acquirerConfiguration is null)
        {
            throw new InvalidOperationException(
                "MerchantAcquirerConfiguration not found for the specified merchant and payment details.");
        }

        if (acquirerConfiguration.Status != MerchantAcquirerConfigurationStatus.Active)
            throw new InvalidOperationException("MerchantAcquirerConfiguration is not active.");

        var merchantPricing = await _merchantPricingRepository.GetActiveByMerchantAndPaymentDetailsAsync(
            command.MerchantId,
            command.PaymentMethodId,
            command.PaymentChannelId,
            currency,
            cancellationToken);

        if (merchantPricing is null)
            throw new InvalidOperationException("MerchantPricing not found for the specified merchant and payment details.");

        if (merchantPricing.Status != MerchantPricingStatus.Active)
            throw new InvalidOperationException("MerchantPricing is not active.");

        var existingIdempotency = await _paymentIdempotencyRepository.GetByMerchantAndKeyAsync(
            command.MerchantId,
            idempotencyKey,
            cancellationToken);

        if (existingIdempotency is not null)
        {
            throw new InvalidOperationException(
                $"IdempotencyKey {idempotencyKey} already exists for merchant {command.MerchantId}.");
        }

        var requestHash = GenerateRequestHash(requestPayloadJson);

        var (grossAmount, feeAmount, vatAmount, taxAmount, otherChargesAmount, netAmount) =
            CalculateAmounts(command.BaseAmount, command.TipAmount, merchantPricing);

        var transactionNumber = GenerateTransactionNumber();

        var idempotency = PaymentIdempotency.Create(
            merchantId: command.MerchantId,
            idempotencyKey: idempotencyKey,
            requestHash: requestHash,
            createdAtUtc: now,
            createdAtLocal: nowLocal,
            expiresAtUtc: null,
            expiresAtLocal: null);

        var transaction = PaymentTransaction.Create(
            transactionNumber: transactionNumber,
            merchantId: command.MerchantId,
            merchantBranchId: command.MerchantBranchId,
            paymentMethodId: command.PaymentMethodId,
            paymentChannelId: command.PaymentChannelId,
            merchantPaymentMethodId: merchantPaymentMethod.MerchantPaymentMethodId,
            merchantAcquirerConfigurationId: acquirerConfiguration.MerchantAcquirerConfigurationId,
            acquirerId: acquirerConfiguration.AcquirerId,
            idempotencyKey: idempotencyKey,
            currency: currency,
            baseAmount: command.BaseAmount,
            tipAmount: command.TipAmount,
            grossAmount: grossAmount,
            feeAmount: feeAmount,
            vatAmount: vatAmount,
            taxAmount: taxAmount,
            otherChargesAmount: otherChargesAmount,
            netAmount: netAmount,
            payerFullName: payerFullName,
            payerRut: payerRut,
            issuingBankName: PendingIssuingBankName,
            cardBrand: PendingCardBrand,
            cardLast4: PendingCardLast4,
            maskedPan: PendingMaskedPan,
            correlationId: command.CorrelationId,
            createdAtUtc: now,
            createdAtLocal: nowLocal);

        await _paymentTransactionRepository.AddAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (transaction.PaymentTransactionId <= 0)
            throw new InvalidOperationException("PaymentTransactionId was not generated after saving the transaction.");

        idempotency.AssignTransaction(transaction.PaymentTransactionId, now, nowLocal);
        await _paymentIdempotencyRepository.AddAsync(idempotency, cancellationToken);

        var pendingStatusHistory = PaymentTransactionStatusHistory.Create(
            paymentTransactionId: transaction.PaymentTransactionId,
            status: PaymentTransactionStatus.Pending,
            statusReasonCode: string.Empty,
            statusReasonDetail: "Payment created.",
            acquirerResponseCode: string.Empty,
            acquirerResponseMessage: string.Empty,
            correlationId: command.CorrelationId,
            createdAtUtc: now,
            createdAtLocal: nowLocal);

        await _paymentTransactionStatusHistoryRepository.AddAsync(pendingStatusHistory, cancellationToken);

        var paymentRequest = PaymentRequest.Create(
            paymentTransactionId: transaction.PaymentTransactionId,
            merchantId: command.MerchantId,
            requestPayloadJson: requestPayloadJson,
            requestHash: requestHash,
            correlationId: command.CorrelationId,
            createdAtUtc: now,
            createdAtLocal: nowLocal);

        await _paymentRequestRepository.AddAsync(paymentRequest, cancellationToken);

        var initialTraceLog = PaymentTraceLog.Create(
            paymentTransactionId: transaction.PaymentTransactionId,
            merchantId: command.MerchantId,
            correlationId: command.CorrelationId,
            eventType: "PaymentCreated",
            eventSource: nameof(CreatePaymentCommandHandler),
            eventDescription: "Payment processing initiated.",
            eventDataJson: JsonSerializer.Serialize(new
            {
                transaction.TransactionNumber,
                transaction.CurrentStatus,
                transaction.GrossAmount,
                transaction.NetAmount
            }),
            severity: TraceSeverity.Information,
            createdAtUtc: now,
            createdAtLocal: nowLocal);

        await _paymentTraceLogRepository.AddAsync(initialTraceLog, cancellationToken);

        transaction.MarkAsProcessing(now, nowLocal);

        var processingStatusHistory = PaymentTransactionStatusHistory.Create(
            paymentTransactionId: transaction.PaymentTransactionId,
            status: PaymentTransactionStatus.Processing,
            statusReasonCode: string.Empty,
            statusReasonDetail: "Sending transaction to bank authorization service.",
            acquirerResponseCode: string.Empty,
            acquirerResponseMessage: string.Empty,
            correlationId: command.CorrelationId,
            createdAtUtc: now,
            createdAtLocal: nowLocal);

        await _paymentTransactionStatusHistoryRepository.AddAsync(processingStatusHistory, cancellationToken);

        var authorizationRequest = new BankTransactionAuthorizationRequest
        {
            PaymentTransactionId = transaction.PaymentTransactionId,
            TransactionNumber = transaction.TransactionNumber,
            MerchantId = command.MerchantId,
            MerchantBranchId = command.MerchantBranchId,
            PaymentMethodId = command.PaymentMethodId,
            PaymentChannelId = command.PaymentChannelId,
            AcquirerId = acquirerConfiguration.AcquirerId,
            Currency = currency,
            GrossAmount = grossAmount,
            PayerFullName = payerFullName,
            PayerRut = payerRut,
            CorrelationId = command.CorrelationId
        };

        try
        {
            var authorizationResult = await _bankAuthorizationService.AuthorizeTransactionAsync(
                authorizationRequest,
                cancellationToken);

            if (authorizationResult.IsApproved)
            {
                transaction.MarkAsApproved(authorizationResult.AcquirerReference, now, nowLocal);

                var approvedStatusHistory = PaymentTransactionStatusHistory.Create(
                    paymentTransactionId: transaction.PaymentTransactionId,
                    status: PaymentTransactionStatus.Approved,
                    statusReasonCode: string.Empty,
                    statusReasonDetail: "Payment approved by bank authorization service.",
                    acquirerResponseCode: authorizationResult.ResponseCode ?? string.Empty,
                    acquirerResponseMessage: authorizationResult.ResponseMessage ?? string.Empty,
                    correlationId: command.CorrelationId,
                    createdAtUtc: now,
                    createdAtLocal: nowLocal);

                await _paymentTransactionStatusHistoryRepository.AddAsync(approvedStatusHistory, cancellationToken);

                idempotency.MarkAsCompleted(now, nowLocal);

                var approvedTraceLog = PaymentTraceLog.Create(
                    paymentTransactionId: transaction.PaymentTransactionId,
                    merchantId: command.MerchantId,
                    correlationId: command.CorrelationId,
                    eventType: "PaymentApproved",
                    eventSource: nameof(CreatePaymentCommandHandler),
                    eventDescription: "Payment approved by bank authorization service.",
                    eventDataJson: JsonSerializer.Serialize(new
                    {
                        authorizationResult.AcquirerReference,
                        authorizationResult.ResponseCode,
                        authorizationResult.ResponseMessage
                    }),
                    severity: TraceSeverity.Information,
                    createdAtUtc: now,
                    createdAtLocal: nowLocal);

                await _paymentTraceLogRepository.AddAsync(approvedTraceLog, cancellationToken);
            }
            else
            {
                transaction.MarkAsDeclined(authorizationResult.AcquirerReference, now, nowLocal);

                var declinedStatusHistory = PaymentTransactionStatusHistory.Create(
                    paymentTransactionId: transaction.PaymentTransactionId,
                    status: PaymentTransactionStatus.Declined,
                    statusReasonCode: string.Empty,
                    statusReasonDetail: "Payment declined by bank authorization service.",
                    acquirerResponseCode: authorizationResult.ResponseCode ?? string.Empty,
                    acquirerResponseMessage: authorizationResult.ResponseMessage ?? string.Empty,
                    correlationId: command.CorrelationId,
                    createdAtUtc: now,
                    createdAtLocal: nowLocal);

                await _paymentTransactionStatusHistoryRepository.AddAsync(declinedStatusHistory, cancellationToken);

                idempotency.MarkAsCompleted(now, nowLocal);

                var declinedTraceLog = PaymentTraceLog.Create(
                    paymentTransactionId: transaction.PaymentTransactionId,
                    merchantId: command.MerchantId,
                    correlationId: command.CorrelationId,
                    eventType: "PaymentDeclined",
                    eventSource: nameof(CreatePaymentCommandHandler),
                    eventDescription: "Payment declined by bank authorization service.",
                    eventDataJson: JsonSerializer.Serialize(new
                    {
                        authorizationResult.AcquirerReference,
                        authorizationResult.ResponseCode,
                        authorizationResult.ResponseMessage
                    }),
                    severity: TraceSeverity.Warning,
                    createdAtUtc: now,
                    createdAtLocal: nowLocal);

                await _paymentTraceLogRepository.AddAsync(declinedTraceLog, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            transaction.MarkAsFailed(now, nowLocal);

            var failedStatusHistory = PaymentTransactionStatusHistory.Create(
                paymentTransactionId: transaction.PaymentTransactionId,
                status: PaymentTransactionStatus.Failed,
                statusReasonCode: "AUTHORIZATION_ERROR",
                statusReasonDetail: "Bank authorization failed.",
                acquirerResponseCode: string.Empty,
                acquirerResponseMessage: string.Empty,
                correlationId: command.CorrelationId,
                createdAtUtc: now,
                createdAtLocal: nowLocal);

            await _paymentTransactionStatusHistoryRepository.AddAsync(failedStatusHistory, cancellationToken);

            idempotency.MarkAsFailed(now, nowLocal);

            var errorTraceLog = PaymentTraceLog.Create(
                paymentTransactionId: transaction.PaymentTransactionId,
                merchantId: command.MerchantId,
                correlationId: command.CorrelationId,
                eventType: "PaymentAuthorizationFailed",
                eventSource: nameof(CreatePaymentCommandHandler),
                eventDescription: "Bank authorization failed.",
                eventDataJson: string.Empty,
                severity: TraceSeverity.Error,
                createdAtUtc: now,
                createdAtLocal: nowLocal);

            await _paymentTraceLogRepository.AddAsync(errorTraceLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw;
        }

        return new CreatePaymentResult
        {
            PaymentTransactionId = transaction.PaymentTransactionId,
            TransactionNumber = transaction.TransactionNumber,
            Status = transaction.CurrentStatus,
            GrossAmount = grossAmount,
            NetAmount = netAmount,
            AcquirerReference = transaction.AcquirerReference,
            CorrelationId = command.CorrelationId
        };
    }

    private static (
        decimal grossAmount,
        decimal feeAmount,
        decimal vatAmount,
        decimal taxAmount,
        decimal otherChargesAmount,
        decimal netAmount)
        CalculateAmounts(
            decimal baseAmount,
            decimal tipAmount,
            MerchantPricing pricing)
    {
        var grossAmount = baseAmount + tipAmount;
        var feeAmount = (grossAmount * (pricing.VariableFeePercentage / 100m)) + pricing.FixedFeeAmount;
        var vatAmount = feeAmount * (pricing.VatPercentage / 100m);
        var taxAmount = grossAmount * (pricing.TaxPercentage / 100m);
        var otherChargesAmount = grossAmount * (pricing.OtherChargePercentage / 100m);
        var netAmount = grossAmount - feeAmount - vatAmount - taxAmount - otherChargesAmount;

        return (grossAmount, feeAmount, vatAmount, taxAmount, otherChargesAmount, netAmount);
    }

    private static string GenerateRequestHash(string requestPayloadJson)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(requestPayloadJson));
        return Convert.ToBase64String(hashBytes);
    }

    private static string GenerateTransactionNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        return $"TXN{timestamp}{suffix}";
    }
}
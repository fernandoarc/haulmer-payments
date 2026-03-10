using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Domain.UnitTests.Payments;

public class PaymentTransactionTests
{
    private const string ValidTransactionNumber = "TXN001";
    private const long ValidMerchantId = 1;
    private const long ValidMerchantBranchId = 1;
    private const long ValidPaymentMethodId = 1;
    private const long ValidPaymentChannelId = 1;
    private const long ValidMerchantPaymentMethodId = 1;
    private const long ValidMerchantAcquirerConfigurationId = 1;
    private const long ValidAcquirerId = 1;
    private const string ValidIdempotencyKey = "IDEMP001";
    private const string ValidCurrency = "CLP";
    private const decimal ValidBaseAmount = 10000.00m;
    private const decimal ValidTipAmount = 1000.00m;
    private const decimal ValidGrossAmount = 11000.00m;
    private const decimal ValidFeeAmount = 200.00m;
    private const decimal ValidVatAmount = 38.00m;
    private const decimal ValidTaxAmount = 0.00m;
    private const decimal ValidOtherChargesAmount = 50.00m;
    private const decimal ValidNetAmount = 10712.00m;
    private const string ValidPayerFullName = "Juan Perez";
    private const string ValidPayerRut = "12.345.678-9";
    private const string ValidIssuingBankName = "Banco Estado";
    private const string ValidCardBrand = "Visa";
    private const string ValidCardLast4 = "1234";
    private const string ValidMaskedPan = "411111******1234";
    private static readonly Guid ValidCorrelationId = Guid.NewGuid();

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var transactionNumber = ValidTransactionNumber;
        var merchantId = ValidMerchantId;
        var merchantBranchId = ValidMerchantBranchId;
        var paymentMethodId = ValidPaymentMethodId;
        var paymentChannelId = ValidPaymentChannelId;
        var merchantPaymentMethodId = ValidMerchantPaymentMethodId;
        var merchantAcquirerConfigurationId = ValidMerchantAcquirerConfigurationId;
        var acquirerId = ValidAcquirerId;
        var idempotencyKey = ValidIdempotencyKey;
        var currency = ValidCurrency;
        var baseAmount = ValidBaseAmount;
        var tipAmount = ValidTipAmount;
        var grossAmount = ValidGrossAmount;
        var feeAmount = ValidFeeAmount;
        var vatAmount = ValidVatAmount;
        var taxAmount = ValidTaxAmount;
        var otherChargesAmount = ValidOtherChargesAmount;
        var netAmount = ValidNetAmount;
        var payerFullName = ValidPayerFullName;
        var payerRut = ValidPayerRut;
        var issuingBankName = ValidIssuingBankName;
        var cardBrand = ValidCardBrand;
        var cardLast4 = ValidCardLast4;
        var maskedPan = ValidMaskedPan;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var paymentTransaction = PaymentTransaction.Create(
            transactionNumber,
            merchantId,
            merchantBranchId,
            paymentMethodId,
            paymentChannelId,
            merchantPaymentMethodId,
            merchantAcquirerConfigurationId,
            acquirerId,
            idempotencyKey,
            currency,
            baseAmount,
            tipAmount,
            grossAmount,
            feeAmount,
            vatAmount,
            taxAmount,
            otherChargesAmount,
            netAmount,
            payerFullName,
            payerRut,
            issuingBankName,
            cardBrand,
            cardLast4,
            maskedPan,
            correlationId,
            createdAtUtc,
            createdAtLocal);

        // Assert
        paymentTransaction.PaymentTransactionId.Should().Be(0); // Default for new entity
        paymentTransaction.TransactionNumber.Should().Be(transactionNumber);
        paymentTransaction.MerchantId.Should().Be(merchantId);
        paymentTransaction.MerchantBranchId.Should().Be(merchantBranchId);
        paymentTransaction.PaymentMethodId.Should().Be(paymentMethodId);
        paymentTransaction.PaymentChannelId.Should().Be(paymentChannelId);
        paymentTransaction.MerchantPaymentMethodId.Should().Be(merchantPaymentMethodId);
        paymentTransaction.MerchantAcquirerConfigurationId.Should().Be(merchantAcquirerConfigurationId);
        paymentTransaction.AcquirerId.Should().Be(acquirerId);
        paymentTransaction.IdempotencyKey.Should().Be(idempotencyKey);
        paymentTransaction.Currency.Should().Be(currency);
        paymentTransaction.BaseAmount.Should().Be(baseAmount);
        paymentTransaction.TipAmount.Should().Be(tipAmount);
        paymentTransaction.GrossAmount.Should().Be(grossAmount);
        paymentTransaction.FeeAmount.Should().Be(feeAmount);
        paymentTransaction.VatAmount.Should().Be(vatAmount);
        paymentTransaction.TaxAmount.Should().Be(taxAmount);
        paymentTransaction.OtherChargesAmount.Should().Be(otherChargesAmount);
        paymentTransaction.NetAmount.Should().Be(netAmount);
        paymentTransaction.PayerFullName.Should().Be(payerFullName);
        paymentTransaction.PayerRut.Should().Be(payerRut);
        paymentTransaction.IssuingBankName.Should().Be(issuingBankName);
        paymentTransaction.CardBrand.Should().Be(cardBrand);
        paymentTransaction.CardLast4.Should().Be(cardLast4);
        paymentTransaction.MaskedPan.Should().Be(maskedPan);
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Pending);
        paymentTransaction.AcquirerReference.Should().BeEmpty();
        paymentTransaction.CorrelationId.Should().Be(correlationId);
        paymentTransaction.CreatedAtUtc.Should().Be(createdAtUtc);
        paymentTransaction.CreatedAtLocal.Should().Be(createdAtLocal);
        paymentTransaction.UpdatedAtUtc.Should().BeNull();
        paymentTransaction.UpdatedAtLocal.Should().BeNull();
        paymentTransaction.ProcessedAtUtc.Should().BeNull();
        paymentTransaction.ProcessedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_TransactionNumberIsEmpty()
    {
        // Arrange
        var transactionNumber = string.Empty;

        // Act
        var act = () => PaymentTransaction.Create(
            transactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("transactionNumber is required. (Parameter 'transactionNumber')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            merchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantId must be greater than zero. (Parameter 'merchantId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantBranchIdIsInvalid()
    {
        // Arrange
        var merchantBranchId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            merchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantBranchId must be greater than zero. (Parameter 'merchantBranchId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PaymentMethodIdIsInvalid()
    {
        // Arrange
        var paymentMethodId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            paymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentMethodId must be greater than zero. (Parameter 'paymentMethodId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PaymentChannelIdIsInvalid()
    {
        // Arrange
        var paymentChannelId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            paymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentChannelId must be greater than zero. (Parameter 'paymentChannelId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantPaymentMethodIdIsInvalid()
    {
        // Arrange
        var merchantPaymentMethodId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            merchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantPaymentMethodId must be greater than zero. (Parameter 'merchantPaymentMethodId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantAcquirerConfigurationIdIsInvalid()
    {
        // Arrange
        var merchantAcquirerConfigurationId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            merchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantAcquirerConfigurationId must be greater than zero. (Parameter 'merchantAcquirerConfigurationId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_AcquirerIdIsInvalid()
    {
        // Arrange
        var acquirerId = 0L;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            acquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("AcquirerId must be greater than zero. (Parameter 'acquirerId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_IdempotencyKeyIsEmpty()
    {
        // Arrange
        var idempotencyKey = string.Empty;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            idempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("idempotencyKey is required. (Parameter 'idempotencyKey')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CurrencyIsEmpty()
    {
        // Arrange
        var currency = string.Empty;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            currency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("currency is required. (Parameter 'currency')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_BaseAmountIsInvalid()
    {
        // Arrange
        var baseAmount = 0m;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            baseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("BaseAmount must be greater than zero. (Parameter 'baseAmount')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_TipAmountIsNegative()
    {
        // Arrange
        var tipAmount = -1m;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            tipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("TipAmount must be greater than or equal to zero. (Parameter 'tipAmount')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_GrossAmountDoesNotMatchBasePlusTip()
    {
        // Arrange
        var grossAmount = 9999m; // Not equal to BaseAmount + TipAmount

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            grossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("GrossAmount must equal BaseAmount + TipAmount. (Parameter 'grossAmount')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CorrelationIdIsEmpty()
    {
        // Arrange
        var correlationId = Guid.Empty;

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            correlationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CorrelationId must not be empty. (Parameter 'correlationId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            createdAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtUtc is required. (Parameter 'createdAtUtc')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtLocalIsDefault()
    {
        // Arrange
        var createdAtLocal = default(DateTime);

        // Act
        var act = () => PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void Create_Should_SetStatusToPending_When_InputIsValid()
    {
        // Arrange & Act
        var paymentTransaction = CreateValidPaymentTransaction();

        // Assert
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Pending);
    }

    [Fact]
    public void MarkAsProcessing_Should_SetStatusToProcessing_When_CurrentStatusIsPending()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction(); // Starts as Pending
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        paymentTransaction.MarkAsProcessing(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Processing);
        paymentTransaction.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsApproved_Should_SetStatusToApprovedAndProcessedDates_When_CurrentStatusIsProcessing()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Processing
        var acquirerReference = "APPROVED123";
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentTransaction.MarkAsApproved(acquirerReference, updatedAtUtc, updatedAtLocal);

        // Assert
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Approved);
        paymentTransaction.AcquirerReference.Should().Be(acquirerReference);
        paymentTransaction.ProcessedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.ProcessedAtLocal.Should().Be(updatedAtLocal);
        paymentTransaction.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsDeclined_Should_SetStatusToDeclinedAndProcessedDates_When_CurrentStatusIsProcessing()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Processing
        var acquirerReference = "DECLINED456";
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentTransaction.MarkAsDeclined(acquirerReference, updatedAtUtc, updatedAtLocal);

        // Assert
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Declined);
        paymentTransaction.AcquirerReference.Should().Be(acquirerReference);
        paymentTransaction.ProcessedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.ProcessedAtLocal.Should().Be(updatedAtLocal);
        paymentTransaction.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsFailed_Should_SetStatusToFailedAndProcessedDates_When_CurrentStatusIsProcessing()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Processing
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentTransaction.MarkAsFailed(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentTransaction.CurrentStatus.Should().Be(PaymentTransactionStatus.Failed);
        paymentTransaction.ProcessedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.ProcessedAtLocal.Should().Be(updatedAtLocal);
        paymentTransaction.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentTransaction.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsApproved_Should_ThrowInvalidOperationException_When_CurrentStatusIsTerminal()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal);
        paymentTransaction.MarkAsApproved("REF", ValidUpdatedAtUtc.AddDays(1), ValidUpdatedAtLocal.AddDays(1)); // Set to Approved (terminal)

        // Act
        var act = () => paymentTransaction.MarkAsApproved("REF2", ValidUpdatedAtUtc.AddDays(2), ValidUpdatedAtLocal.AddDays(2));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot mark as approved from current status.");
    }

    [Fact]
    public void MarkAsDeclined_Should_ThrowInvalidOperationException_When_CurrentStatusIsTerminal()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal);
        paymentTransaction.MarkAsApproved("REF", ValidUpdatedAtUtc.AddDays(1), ValidUpdatedAtLocal.AddDays(1)); // Set to Approved (terminal)

        // Act
        var act = () => paymentTransaction.MarkAsDeclined("REF2", ValidUpdatedAtUtc.AddDays(2), ValidUpdatedAtLocal.AddDays(2));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot mark as declined from current status.");
    }

    [Fact]
    public void MarkAsFailed_Should_ThrowInvalidOperationException_When_CurrentStatusIsTerminal()
    {
        // Arrange
        var paymentTransaction = CreateValidPaymentTransaction();
        paymentTransaction.MarkAsProcessing(ValidUpdatedAtUtc, ValidUpdatedAtLocal);
        paymentTransaction.MarkAsApproved("REF", ValidUpdatedAtUtc.AddDays(1), ValidUpdatedAtLocal.AddDays(1)); // Set to Approved (terminal)

        // Act
        var act = () => paymentTransaction.MarkAsFailed(ValidUpdatedAtUtc.AddDays(2), ValidUpdatedAtLocal.AddDays(2));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot mark as failed from current status.");
    }

    private static PaymentTransaction CreateValidPaymentTransaction()
    {
        return PaymentTransaction.Create(
            ValidTransactionNumber,
            ValidMerchantId,
            ValidMerchantBranchId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidMerchantPaymentMethodId,
            ValidMerchantAcquirerConfigurationId,
            ValidAcquirerId,
            ValidIdempotencyKey,
            ValidCurrency,
            ValidBaseAmount,
            ValidTipAmount,
            ValidGrossAmount,
            ValidFeeAmount,
            ValidVatAmount,
            ValidTaxAmount,
            ValidOtherChargesAmount,
            ValidNetAmount,
            ValidPayerFullName,
            ValidPayerRut,
            ValidIssuingBankName,
            ValidCardBrand,
            ValidCardLast4,
            ValidMaskedPan,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
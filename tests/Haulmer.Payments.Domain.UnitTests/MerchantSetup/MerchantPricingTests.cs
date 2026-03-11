using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Domain.UnitTests.MerchantSetup;

public class MerchantPricingTests
{
    private const long ValidMerchantId = 1;
    private const long ValidPaymentMethodId = 1;
    private const long ValidPaymentChannelId = 1;
    private const string ValidCurrency = "CLP";
    private const decimal ValidFixedFeeAmount = 100.00m;
    private const decimal ValidVariableFeePercentage = 2.5m;
    private const decimal ValidVatPercentage = 19.0m;
    private const decimal ValidTaxPercentage = 0.0m;
    private const decimal ValidOtherChargePercentage = 1.0m;

    private static DateTime ValidValidFromUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidValidFromLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var merchantId = ValidMerchantId;
        var paymentMethodId = ValidPaymentMethodId;
        var paymentChannelId = ValidPaymentChannelId;
        var currency = ValidCurrency;
        var fixedFeeAmount = ValidFixedFeeAmount;
        var variableFeePercentage = ValidVariableFeePercentage;
        var vatPercentage = ValidVatPercentage;
        var taxPercentage = ValidTaxPercentage;
        var otherChargePercentage = ValidOtherChargePercentage;
        var validFromUtc = ValidValidFromUtc;
        var validFromLocal = ValidValidFromLocal;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var merchantPricing = MerchantPricing.Create(
            merchantId,
            paymentMethodId,
            paymentChannelId,
            currency,
            fixedFeeAmount,
            variableFeePercentage,
            vatPercentage,
            taxPercentage,
            otherChargePercentage,
            validFromUtc,
            validFromLocal,
            createdAtUtc,
            createdAtLocal);

        // Assert
        merchantPricing.MerchantPricingId.Should().Be(0); // Default for new entity
        merchantPricing.MerchantId.Should().Be(merchantId);
        merchantPricing.PaymentMethodId.Should().Be(paymentMethodId);
        merchantPricing.PaymentChannelId.Should().Be(paymentChannelId);
        merchantPricing.Currency.Should().Be(currency);
        merchantPricing.FixedFeeAmount.Should().Be(fixedFeeAmount);
        merchantPricing.VariableFeePercentage.Should().Be(variableFeePercentage);
        merchantPricing.VatPercentage.Should().Be(vatPercentage);
        merchantPricing.TaxPercentage.Should().Be(taxPercentage);
        merchantPricing.OtherChargePercentage.Should().Be(otherChargePercentage);
        merchantPricing.ValidFromUtc.Should().Be(validFromUtc);
        merchantPricing.ValidFromLocal.Should().Be(validFromLocal);
        merchantPricing.ValidToUtc.Should().BeNull();
        merchantPricing.ValidToLocal.Should().BeNull();
        merchantPricing.Status.Should().Be(MerchantPricingStatus.Active);
        merchantPricing.CreatedAtUtc.Should().Be(createdAtUtc);
        merchantPricing.CreatedAtLocal.Should().Be(createdAtLocal);
        merchantPricing.UpdatedAtUtc.Should().BeNull();
        merchantPricing.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => MerchantPricing.Create(
            merchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantId must be greater than zero. (Parameter 'merchantId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PaymentMethodIdIsInvalid()
    {
        // Arrange
        var paymentMethodId = 0L;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            paymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
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
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            paymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentChannelId must be greater than zero. (Parameter 'paymentChannelId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CurrencyIsEmpty()
    {
        // Arrange
        var currency = string.Empty;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            currency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("currency is required. (Parameter 'currency')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_FixedFeeAmountIsNegative()
    {
        // Arrange
        var fixedFeeAmount = -1.0m;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            fixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("FixedFeeAmount must be greater than or equal to zero. (Parameter 'fixedFeeAmount')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_VariableFeePercentageIsNegative()
    {
        // Arrange
        var variableFeePercentage = -1.0m;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            variableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("VariableFeePercentage must be greater than or equal to zero. (Parameter 'variableFeePercentage')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_VatPercentageIsNegative()
    {
        // Arrange
        var vatPercentage = -1.0m;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            vatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("VatPercentage must be greater than or equal to zero. (Parameter 'vatPercentage')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_TaxPercentageIsNegative()
    {
        // Arrange
        var taxPercentage = -1.0m;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            taxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("TaxPercentage must be greater than or equal to zero. (Parameter 'taxPercentage')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_OtherChargePercentageIsNegative()
    {
        // Arrange
        var otherChargePercentage = -1.0m;

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            otherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("OtherChargePercentage must be greater than or equal to zero. (Parameter 'otherChargePercentage')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_ValidFromUtcIsDefault()
    {
        // Arrange
        var validFromUtc = default(DateTime);

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            validFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.Message.Should().StartWith("validFromUtc is required.");
        exception.ParamName.Should().Be("validFromUtc");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_ValidFromLocalIsDefault()
    {
        // Arrange
        var validFromLocal = default(DateTime);

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            validFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.Message.Should().StartWith("validFromLocal is required.");
        exception.ParamName.Should().Be("validFromLocal");
    }
    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
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
        var act = () => MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void Activate_Should_SetStatusToActiveAndClearValidToDates_When_CurrentStatusIsNotActive()
    {
        // Arrange
        var merchantPricing = CreateValidMerchantPricing();
        merchantPricing.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive and set ValidTo dates
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantPricing.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPricing.Status.Should().Be(MerchantPricingStatus.Active);
        merchantPricing.ValidToUtc.Should().BeNull();
        merchantPricing.ValidToLocal.Should().BeNull();
        merchantPricing.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantPricing.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactiveAndSetValidToDates_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var merchantPricing = CreateValidMerchantPricing(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantPricing.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPricing.Status.Should().Be(MerchantPricingStatus.Inactive);
        merchantPricing.ValidToUtc.Should().Be(updatedAtUtc);
        merchantPricing.ValidToLocal.Should().Be(updatedAtLocal);
        merchantPricing.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantPricing.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var merchantPricing = CreateValidMerchantPricing(); // Starts as Active
        var initialUpdatedAtUtc = merchantPricing.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantPricing.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantPricing.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPricing.Status.Should().Be(MerchantPricingStatus.Active);
        merchantPricing.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantPricing.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var merchantPricing = CreateValidMerchantPricing();
        merchantPricing.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = merchantPricing.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantPricing.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantPricing.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchantPricing.Status.Should().Be(MerchantPricingStatus.Inactive);
        merchantPricing.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantPricing.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static MerchantPricing CreateValidMerchantPricing()
    {
        return MerchantPricing.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidCurrency,
            ValidFixedFeeAmount,
            ValidVariableFeePercentage,
            ValidVatPercentage,
            ValidTaxPercentage,
            ValidOtherChargePercentage,
            ValidValidFromUtc,
            ValidValidFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
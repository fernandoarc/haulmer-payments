using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Domain.UnitTests.MerchantSetup;

public class MerchantAcquirerConfigurationTests
{
    private const long ValidMerchantId = 1;
    private const long ValidPaymentMethodId = 1;
    private const long ValidPaymentChannelId = 1;
    private const long ValidAcquirerId = 1;
    private const string ValidCurrency = "CLP";
    private const string ValidMerchantTerminalCode = "TERM001";
    private const string ValidAcquirerMerchantCode = "ACQ001";
    private const int ValidTimeoutSeconds = 30;
    private const int ValidPriority = 1;

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
        var acquirerId = ValidAcquirerId;
        var currency = ValidCurrency;
        var merchantTerminalCode = ValidMerchantTerminalCode;
        var acquirerMerchantCode = ValidAcquirerMerchantCode;
        var timeoutSeconds = ValidTimeoutSeconds;
        var priority = ValidPriority;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var merchantAcquirerConfiguration = MerchantAcquirerConfiguration.Create(
            merchantId,
            paymentMethodId,
            paymentChannelId,
            acquirerId,
            currency,
            merchantTerminalCode,
            acquirerMerchantCode,
            timeoutSeconds,
            priority,
            createdAtUtc,
            createdAtLocal);

        // Assert
        merchantAcquirerConfiguration.MerchantAcquirerConfigurationId.Should().Be(0); // Default for new entity
        merchantAcquirerConfiguration.MerchantId.Should().Be(merchantId);
        merchantAcquirerConfiguration.PaymentMethodId.Should().Be(paymentMethodId);
        merchantAcquirerConfiguration.PaymentChannelId.Should().Be(paymentChannelId);
        merchantAcquirerConfiguration.AcquirerId.Should().Be(acquirerId);
        merchantAcquirerConfiguration.Currency.Should().Be(currency);
        merchantAcquirerConfiguration.MerchantTerminalCode.Should().Be(merchantTerminalCode);
        merchantAcquirerConfiguration.AcquirerMerchantCode.Should().Be(acquirerMerchantCode);
        merchantAcquirerConfiguration.TimeoutSeconds.Should().Be(timeoutSeconds);
        merchantAcquirerConfiguration.Priority.Should().Be(priority);
        merchantAcquirerConfiguration.Status.Should().Be(MerchantAcquirerConfigurationStatus.Active);
        merchantAcquirerConfiguration.CreatedAtUtc.Should().Be(createdAtUtc);
        merchantAcquirerConfiguration.CreatedAtLocal.Should().Be(createdAtLocal);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().BeNull();
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            merchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
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
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            paymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
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
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            paymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentChannelId must be greater than zero. (Parameter 'paymentChannelId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_AcquirerIdIsInvalid()
    {
        // Arrange
        var acquirerId = 0L;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            acquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("AcquirerId must be greater than zero. (Parameter 'acquirerId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CurrencyIsEmpty()
    {
        // Arrange
        var currency = string.Empty;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            currency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("currency is required. (Parameter 'currency')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantTerminalCodeIsEmpty()
    {
        // Arrange
        var merchantTerminalCode = string.Empty;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            merchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("merchantTerminalCode is required. (Parameter 'merchantTerminalCode')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_AcquirerMerchantCodeIsEmpty()
    {
        // Arrange
        var acquirerMerchantCode = string.Empty;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            acquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("acquirerMerchantCode is required. (Parameter 'acquirerMerchantCode')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_TimeoutSecondsIsInvalid()
    {
        // Arrange
        var timeoutSeconds = 0;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            timeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("TimeoutSeconds must be greater than zero. (Parameter 'timeoutSeconds')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PriorityIsInvalid()
    {
        // Arrange
        var priority = 0;

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            priority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Priority must be greater than zero. (Parameter 'priority')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
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
        var act = () => MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void Activate_Should_SetStatusToActive_When_CurrentStatusIsNotActive()
    {
        // Arrange
        var merchantAcquirerConfiguration = CreateValidMerchantAcquirerConfiguration();
        merchantAcquirerConfiguration.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantAcquirerConfiguration.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantAcquirerConfiguration.Status.Should().Be(MerchantAcquirerConfigurationStatus.Active);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var merchantAcquirerConfiguration = CreateValidMerchantAcquirerConfiguration(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantAcquirerConfiguration.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantAcquirerConfiguration.Status.Should().Be(MerchantAcquirerConfigurationStatus.Inactive);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var merchantAcquirerConfiguration = CreateValidMerchantAcquirerConfiguration(); // Starts as Active
        var initialUpdatedAtUtc = merchantAcquirerConfiguration.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantAcquirerConfiguration.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantAcquirerConfiguration.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantAcquirerConfiguration.Status.Should().Be(MerchantAcquirerConfigurationStatus.Active);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var merchantAcquirerConfiguration = CreateValidMerchantAcquirerConfiguration();
        merchantAcquirerConfiguration.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = merchantAcquirerConfiguration.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantAcquirerConfiguration.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantAcquirerConfiguration.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchantAcquirerConfiguration.Status.Should().Be(MerchantAcquirerConfigurationStatus.Inactive);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void UpdateRoutingData_Should_UpdateFieldsAndTimestamps_When_InputIsValid()
    {
        // Arrange
        var merchantAcquirerConfiguration = CreateValidMerchantAcquirerConfiguration();
        var newMerchantTerminalCode = "NEWTERM001";
        var newAcquirerMerchantCode = "NEWACQ001";
        var newTimeoutSeconds = 60;
        var newPriority = 2;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantAcquirerConfiguration.UpdateRoutingData(
            newMerchantTerminalCode,
            newAcquirerMerchantCode,
            newTimeoutSeconds,
            newPriority,
            updatedAtUtc,
            updatedAtLocal);

        // Assert
        merchantAcquirerConfiguration.MerchantTerminalCode.Should().Be(newMerchantTerminalCode);
        merchantAcquirerConfiguration.AcquirerMerchantCode.Should().Be(newAcquirerMerchantCode);
        merchantAcquirerConfiguration.TimeoutSeconds.Should().Be(newTimeoutSeconds);
        merchantAcquirerConfiguration.Priority.Should().Be(newPriority);
        merchantAcquirerConfiguration.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantAcquirerConfiguration.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    private static MerchantAcquirerConfiguration CreateValidMerchantAcquirerConfiguration()
    {
        return MerchantAcquirerConfiguration.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidPaymentChannelId,
            ValidAcquirerId,
            ValidCurrency,
            ValidMerchantTerminalCode,
            ValidAcquirerMerchantCode,
            ValidTimeoutSeconds,
            ValidPriority,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
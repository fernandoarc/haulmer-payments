using Haulmer.Payments.Domain.MerchantSetup;

namespace Haulmer.Payments.Domain.UnitTests.MerchantSetup;

public class MerchantPaymentMethodTests
{
    private const long ValidMerchantId = 1;
    private const long ValidPaymentMethodId = 1;

    private static DateTime ValidEnabledFromUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidEnabledFromLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
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
        var enabledFromUtc = ValidEnabledFromUtc;
        var enabledFromLocal = ValidEnabledFromLocal;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var merchantPaymentMethod = MerchantPaymentMethod.Create(
            merchantId,
            paymentMethodId,
            enabledFromUtc,
            enabledFromLocal,
            createdAtUtc,
            createdAtLocal);

        // Assert
        merchantPaymentMethod.MerchantPaymentMethodId.Should().Be(0); // Default for new entity
        merchantPaymentMethod.MerchantId.Should().Be(merchantId);
        merchantPaymentMethod.PaymentMethodId.Should().Be(paymentMethodId);
        merchantPaymentMethod.Status.Should().Be(MerchantPaymentMethodStatus.Active);
        merchantPaymentMethod.EnabledFromUtc.Should().Be(enabledFromUtc);
        merchantPaymentMethod.EnabledFromLocal.Should().Be(enabledFromLocal);
        merchantPaymentMethod.EnabledToUtc.Should().BeNull();
        merchantPaymentMethod.EnabledToLocal.Should().BeNull();
        merchantPaymentMethod.CreatedAtUtc.Should().Be(createdAtUtc);
        merchantPaymentMethod.CreatedAtLocal.Should().Be(createdAtLocal);
        merchantPaymentMethod.UpdatedAtUtc.Should().BeNull();
        merchantPaymentMethod.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => MerchantPaymentMethod.Create(
            merchantId,
            ValidPaymentMethodId,
            ValidEnabledFromUtc,
            ValidEnabledFromLocal,
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
        var act = () => MerchantPaymentMethod.Create(
            ValidMerchantId,
            paymentMethodId,
            ValidEnabledFromUtc,
            ValidEnabledFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentMethodId must be greater than zero. (Parameter 'paymentMethodId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_EnabledFromUtcIsDefault()
    {
        // Arrange
        var enabledFromUtc = default(DateTime);

        // Act
        var act = () => MerchantPaymentMethod.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            enabledFromUtc,
            ValidEnabledFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.Message.Should().StartWith("enabledFromUtc is required.");
        exception.ParamName.Should().Be("enabledFromUtc");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_EnabledFromLocalIsDefault()
    {
        // Arrange
        var enabledFromLocal = default(DateTime);

        // Act
        var act = () => MerchantPaymentMethod.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidEnabledFromUtc,
            enabledFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        var exception = act.Should().Throw<ArgumentException>().Which;
        exception.Message.Should().StartWith("enabledFromLocal is required.");
        exception.ParamName.Should().Be("enabledFromLocal");
    }
    
    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => MerchantPaymentMethod.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidEnabledFromUtc,
            ValidEnabledFromLocal,
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
        var act = () => MerchantPaymentMethod.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidEnabledFromUtc,
            ValidEnabledFromLocal,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }

    [Fact]
    public void Activate_Should_SetStatusToActiveAndClearDisabledDates_When_CurrentStatusIsNotActive()
    {
        // Arrange
        var merchantPaymentMethod = CreateValidMerchantPaymentMethod();
        merchantPaymentMethod.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive and set EnabledTo dates
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantPaymentMethod.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPaymentMethod.Status.Should().Be(MerchantPaymentMethodStatus.Active);
        merchantPaymentMethod.EnabledToUtc.Should().BeNull();
        merchantPaymentMethod.EnabledToLocal.Should().BeNull();
        merchantPaymentMethod.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantPaymentMethod.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactiveAndSetDisabledDates_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var merchantPaymentMethod = CreateValidMerchantPaymentMethod(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantPaymentMethod.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPaymentMethod.Status.Should().Be(MerchantPaymentMethodStatus.Inactive);
        merchantPaymentMethod.EnabledToUtc.Should().Be(updatedAtUtc);
        merchantPaymentMethod.EnabledToLocal.Should().Be(updatedAtLocal);
        merchantPaymentMethod.UpdatedAtUtc.Should().Be(updatedAtUtc);
        merchantPaymentMethod.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var merchantPaymentMethod = CreateValidMerchantPaymentMethod(); // Starts as Active
        var initialUpdatedAtUtc = merchantPaymentMethod.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantPaymentMethod.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        merchantPaymentMethod.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        merchantPaymentMethod.Status.Should().Be(MerchantPaymentMethodStatus.Active);
        merchantPaymentMethod.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantPaymentMethod.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var merchantPaymentMethod = CreateValidMerchantPaymentMethod();
        merchantPaymentMethod.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = merchantPaymentMethod.UpdatedAtUtc;
        var initialUpdatedAtLocal = merchantPaymentMethod.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        merchantPaymentMethod.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        merchantPaymentMethod.Status.Should().Be(MerchantPaymentMethodStatus.Inactive);
        merchantPaymentMethod.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        merchantPaymentMethod.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static MerchantPaymentMethod CreateValidMerchantPaymentMethod()
    {
        return MerchantPaymentMethod.Create(
            ValidMerchantId,
            ValidPaymentMethodId,
            ValidEnabledFromUtc,
            ValidEnabledFromLocal,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
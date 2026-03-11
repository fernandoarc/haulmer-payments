using Haulmer.Payments.Domain.Catalogs;

namespace Haulmer.Payments.Domain.UnitTests.Catalogs;

public class PaymentMethodTests
{
    private const string ValidCode = "PM001";
    private const string ValidName = "Credit Card";

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static DateTime ValidUpdatedAtUtc => new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidUpdatedAtLocal => new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var code = ValidCode;
        var name = ValidName;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var paymentMethod = PaymentMethod.Create(
            code,
            name,
            createdAtUtc,
            createdAtLocal);

        // Assert
        paymentMethod.PaymentMethodId.Should().Be(0); // Default for new entity
        paymentMethod.Code.Should().Be(code);
        paymentMethod.Name.Should().Be(name);
        paymentMethod.Status.Should().Be(PaymentMethodStatus.Active);
        paymentMethod.CreatedAtUtc.Should().Be(createdAtUtc);
        paymentMethod.CreatedAtLocal.Should().Be(createdAtLocal);
        paymentMethod.UpdatedAtUtc.Should().BeNull();
        paymentMethod.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CodeIsEmpty()
    {
        // Arrange
        var code = string.Empty;

        // Act
        var act = () => PaymentMethod.Create(
            code,
            ValidName,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("code is required. (Parameter 'code')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_NameIsEmpty()
    {
        // Arrange
        var name = string.Empty;

        // Act
        var act = () => PaymentMethod.Create(
            ValidCode,
            name,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("name is required. (Parameter 'name')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => PaymentMethod.Create(
            ValidCode,
            ValidName,
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
        var act = () => PaymentMethod.Create(
            ValidCode,
            ValidName,
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
        var paymentMethod = CreateValidPaymentMethod();
        paymentMethod.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentMethod.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentMethod.Status.Should().Be(PaymentMethodStatus.Active);
        paymentMethod.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentMethod.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var paymentMethod = CreateValidPaymentMethod(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        paymentMethod.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentMethod.Status.Should().Be(PaymentMethodStatus.Inactive);
        paymentMethod.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentMethod.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var paymentMethod = CreateValidPaymentMethod(); // Starts as Active
        var initialUpdatedAtUtc = paymentMethod.UpdatedAtUtc;
        var initialUpdatedAtLocal = paymentMethod.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        paymentMethod.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentMethod.Status.Should().Be(PaymentMethodStatus.Active);
        paymentMethod.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        paymentMethod.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var paymentMethod = CreateValidPaymentMethod();
        paymentMethod.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = paymentMethod.UpdatedAtUtc;
        var initialUpdatedAtLocal = paymentMethod.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentMethod.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        paymentMethod.Status.Should().Be(PaymentMethodStatus.Inactive);
        paymentMethod.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        paymentMethod.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static PaymentMethod CreateValidPaymentMethod()
    {
        return PaymentMethod.Create(
            ValidCode,
            ValidName,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
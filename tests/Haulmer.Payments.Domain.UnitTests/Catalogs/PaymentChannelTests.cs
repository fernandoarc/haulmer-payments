using Haulmer.Payments.Domain.Catalogs;

namespace Haulmer.Payments.Domain.UnitTests.Catalogs;

public class PaymentChannelTests
{
    private const string ValidCode = "PC001";
    private const string ValidName = "Online";

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
        var paymentChannel = PaymentChannel.Create(
            code,
            name,
            createdAtUtc,
            createdAtLocal);

        // Assert
        paymentChannel.PaymentChannelId.Should().Be(0); // Default for new entity
        paymentChannel.Code.Should().Be(code);
        paymentChannel.Name.Should().Be(name);
        paymentChannel.Status.Should().Be(PaymentChannelStatus.Active);
        paymentChannel.CreatedAtUtc.Should().Be(createdAtUtc);
        paymentChannel.CreatedAtLocal.Should().Be(createdAtLocal);
        paymentChannel.UpdatedAtUtc.Should().BeNull();
        paymentChannel.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CodeIsEmpty()
    {
        // Arrange
        var code = string.Empty;

        // Act
        var act = () => PaymentChannel.Create(
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
        var act = () => PaymentChannel.Create(
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
        var act = () => PaymentChannel.Create(
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
        var act = () => PaymentChannel.Create(
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
        var paymentChannel = CreateValidPaymentChannel();
        paymentChannel.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var updatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var updatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentChannel.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentChannel.Status.Should().Be(PaymentChannelStatus.Active);
        paymentChannel.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentChannel.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_SetStatusToInactive_When_CurrentStatusIsNotInactive()
    {
        // Arrange
        var paymentChannel = CreateValidPaymentChannel(); // Starts as Active
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        paymentChannel.Deactivate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentChannel.Status.Should().Be(PaymentChannelStatus.Inactive);
        paymentChannel.UpdatedAtUtc.Should().Be(updatedAtUtc);
        paymentChannel.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void Activate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyActive()
    {
        // Arrange
        var paymentChannel = CreateValidPaymentChannel(); // Starts as Active
        var initialUpdatedAtUtc = paymentChannel.UpdatedAtUtc;
        var initialUpdatedAtLocal = paymentChannel.UpdatedAtLocal;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        paymentChannel.Activate(updatedAtUtc, updatedAtLocal);

        // Assert
        paymentChannel.Status.Should().Be(PaymentChannelStatus.Active);
        paymentChannel.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        paymentChannel.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    [Fact]
    public void Deactivate_Should_NotChangeUpdatedDates_When_StatusIsAlreadyInactive()
    {
        // Arrange
        var paymentChannel = CreateValidPaymentChannel();
        paymentChannel.Deactivate(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Set to Inactive
        var initialUpdatedAtUtc = paymentChannel.UpdatedAtUtc;
        var initialUpdatedAtLocal = paymentChannel.UpdatedAtLocal;
        var newUpdatedAtUtc = ValidUpdatedAtUtc.AddDays(1);
        var newUpdatedAtLocal = ValidUpdatedAtLocal.AddDays(1);

        // Act
        paymentChannel.Deactivate(newUpdatedAtUtc, newUpdatedAtLocal);

        // Assert
        paymentChannel.Status.Should().Be(PaymentChannelStatus.Inactive);
        paymentChannel.UpdatedAtUtc.Should().Be(initialUpdatedAtUtc);
        paymentChannel.UpdatedAtLocal.Should().Be(initialUpdatedAtLocal);
    }

    private static PaymentChannel CreateValidPaymentChannel()
    {
        return PaymentChannel.Create(
            ValidCode,
            ValidName,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);
    }
}
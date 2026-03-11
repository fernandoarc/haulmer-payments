using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Domain.UnitTests.Payments;

public class PaymentIdempotencyTests
{
    private const long ValidMerchantId = 1;
    private const string ValidIdempotencyKey = "IDEMP001";
    private const string ValidRequestHash = "hash123";
    private const long ValidPaymentTransactionId = 1;
    private static readonly DateTime ValidCreatedAtUtc = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ValidCreatedAtLocal = new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);
    private static readonly DateTime ValidExpiresAtUtc = new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ValidExpiresAtLocal = new DateTime(2023, 1, 2, 9, 0, 0, DateTimeKind.Local);
    private static readonly DateTime ValidUpdatedAtUtc = new DateTime(2023, 1, 1, 13, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ValidUpdatedAtLocal = new DateTime(2023, 1, 1, 10, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var merchantId = ValidMerchantId;
        var idempotencyKey = ValidIdempotencyKey;
        var requestHash = ValidRequestHash;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;
        var expiresAtUtc = ValidExpiresAtUtc;
        var expiresAtLocal = ValidExpiresAtLocal;

        // Act
        var idempotency = PaymentIdempotency.Create(
            merchantId,
            idempotencyKey,
            requestHash,
            createdAtUtc,
            createdAtLocal,
            expiresAtUtc,
            expiresAtLocal);

        // Assert
        idempotency.PaymentIdempotencyId.Should().Be(0); // Default for new entity
        idempotency.MerchantId.Should().Be(merchantId);
        idempotency.IdempotencyKey.Should().Be(idempotencyKey);
        idempotency.RequestHash.Should().Be(requestHash);
        idempotency.PaymentTransactionId.Should().BeNull();
        idempotency.Status.Should().Be(PaymentIdempotencyStatus.InProgress);
        idempotency.CreatedAtUtc.Should().Be(createdAtUtc);
        idempotency.CreatedAtLocal.Should().Be(createdAtLocal);
        idempotency.ExpiresAtUtc.Should().Be(expiresAtUtc);
        idempotency.ExpiresAtLocal.Should().Be(expiresAtLocal);
        idempotency.UpdatedAtUtc.Should().BeNull();
        idempotency.UpdatedAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_AllowNullExpiresAtDates()
    {
        // Arrange
        var merchantId = ValidMerchantId;
        var idempotencyKey = ValidIdempotencyKey;
        var requestHash = ValidRequestHash;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;
        DateTime? expiresAtUtc = null;
        DateTime? expiresAtLocal = null;

        // Act
        var idempotency = PaymentIdempotency.Create(
            merchantId,
            idempotencyKey,
            requestHash,
            createdAtUtc,
            createdAtLocal,
            expiresAtUtc,
            expiresAtLocal);

        // Assert
        idempotency.ExpiresAtUtc.Should().BeNull();
        idempotency.ExpiresAtLocal.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => PaymentIdempotency.Create(
            merchantId,
            ValidIdempotencyKey,
            ValidRequestHash,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal,
            ValidExpiresAtUtc,
            ValidExpiresAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantId must be greater than zero. (Parameter 'merchantId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_IdempotencyKeyIsEmpty()
    {
        // Arrange
        var idempotencyKey = string.Empty;

        // Act
        var act = () => PaymentIdempotency.Create(
            ValidMerchantId,
            idempotencyKey,
            ValidRequestHash,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal,
            ValidExpiresAtUtc,
            ValidExpiresAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("idempotencyKey is required. (Parameter 'idempotencyKey')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_RequestHashIsEmpty()
    {
        // Arrange
        var requestHash = string.Empty;

        // Act
        var act = () => PaymentIdempotency.Create(
            ValidMerchantId,
            ValidIdempotencyKey,
            requestHash,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal,
            ValidExpiresAtUtc,
            ValidExpiresAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("requestHash is required. (Parameter 'requestHash')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CreatedAtUtcIsDefault()
    {
        // Arrange
        var createdAtUtc = default(DateTime);

        // Act
        var act = () => PaymentIdempotency.Create(
            ValidMerchantId,
            ValidIdempotencyKey,
            ValidRequestHash,
            createdAtUtc,
            ValidCreatedAtLocal,
            ValidExpiresAtUtc,
            ValidExpiresAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtUtc is required. (Parameter 'createdAtUtc')");
    }

    [Fact]
    public void AssignTransaction_Should_SetPaymentTransactionIdAndUpdateTimestamps_When_StatusIsInProgress()
    {
        // Arrange
        var idempotency = CreateValidPaymentIdempotency();
        var paymentTransactionId = ValidPaymentTransactionId;
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        idempotency.AssignTransaction(paymentTransactionId, updatedAtUtc, updatedAtLocal);

        // Assert
        idempotency.PaymentTransactionId.Should().Be(paymentTransactionId);
        idempotency.UpdatedAtUtc.Should().Be(updatedAtUtc);
        idempotency.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsCompleted_Should_SetStatusToCompleted_When_StatusIsInProgress()
    {
        // Arrange
        var idempotency = CreateValidPaymentIdempotency();
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        idempotency.MarkAsCompleted(updatedAtUtc, updatedAtLocal);

        // Assert
        idempotency.Status.Should().Be(PaymentIdempotencyStatus.Completed);
        idempotency.UpdatedAtUtc.Should().Be(updatedAtUtc);
        idempotency.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsFailed_Should_SetStatusToFailed_When_StatusIsInProgress()
    {
        // Arrange
        var idempotency = CreateValidPaymentIdempotency();
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        idempotency.MarkAsFailed(updatedAtUtc, updatedAtLocal);

        // Assert
        idempotency.Status.Should().Be(PaymentIdempotencyStatus.Failed);
        idempotency.UpdatedAtUtc.Should().Be(updatedAtUtc);
        idempotency.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsExpired_Should_SetStatusToExpired_When_StatusIsInProgress()
    {
        // Arrange
        var idempotency = CreateValidPaymentIdempotency();
        var updatedAtUtc = ValidUpdatedAtUtc;
        var updatedAtLocal = ValidUpdatedAtLocal;

        // Act
        idempotency.MarkAsExpired(updatedAtUtc, updatedAtLocal);

        // Assert
        idempotency.Status.Should().Be(PaymentIdempotencyStatus.Expired);
        idempotency.UpdatedAtUtc.Should().Be(updatedAtUtc);
        idempotency.UpdatedAtLocal.Should().Be(updatedAtLocal);
    }

    [Fact]
    public void MarkAsCompleted_Should_ThrowInvalidOperationException_When_StatusIsNotInProgress()
    {
        // Arrange
        var idempotency = CreateValidPaymentIdempotency();
        idempotency.MarkAsCompleted(ValidUpdatedAtUtc, ValidUpdatedAtLocal); // Change to Completed

        // Act
        var act = () => idempotency.MarkAsCompleted(ValidUpdatedAtUtc.AddHours(1), ValidUpdatedAtLocal.AddHours(1));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot mark as completed from current status.");
    }

    private static PaymentIdempotency CreateValidPaymentIdempotency()
    {
        return PaymentIdempotency.Create(
            ValidMerchantId,
            ValidIdempotencyKey,
            ValidRequestHash,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal,
            ValidExpiresAtUtc,
            ValidExpiresAtLocal);
    }
}
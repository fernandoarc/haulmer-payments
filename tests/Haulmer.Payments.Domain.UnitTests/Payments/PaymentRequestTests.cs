using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Domain.UnitTests.Payments;

public class PaymentRequestTests
{
    private const long ValidPaymentTransactionId = 1;
    private const long ValidMerchantId = 1;
    private const string ValidRequestPayloadJson = "{\"amount\":10000,\"currency\":\"CLP\",\"payer\":\"Juan Perez\"}";
    private const string ValidRequestHash = "abc123def456";
    private static readonly Guid ValidCorrelationId = Guid.NewGuid();

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var paymentTransactionId = ValidPaymentTransactionId;
        var merchantId = ValidMerchantId;
        var requestPayloadJson = ValidRequestPayloadJson;
        var requestHash = ValidRequestHash;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var paymentRequest = PaymentRequest.Create(
            paymentTransactionId,
            merchantId,
            requestPayloadJson,
            requestHash,
            correlationId,
            createdAtUtc,
            createdAtLocal);

        // Assert
        paymentRequest.PaymentRequestId.Should().Be(0); // Default for new entity
        paymentRequest.PaymentTransactionId.Should().Be(paymentTransactionId);
        paymentRequest.MerchantId.Should().Be(merchantId);
        paymentRequest.RequestPayloadJson.Should().Be(requestPayloadJson);
        paymentRequest.RequestHash.Should().Be(requestHash);
        paymentRequest.CorrelationId.Should().Be(correlationId);
        paymentRequest.CreatedAtUtc.Should().Be(createdAtUtc);
        paymentRequest.CreatedAtLocal.Should().Be(createdAtLocal);
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PaymentTransactionIdIsInvalid()
    {
        // Arrange
        var paymentTransactionId = 0L;

        // Act
        var act = () => PaymentRequest.Create(
            paymentTransactionId,
            ValidMerchantId,
            ValidRequestPayloadJson,
            ValidRequestHash,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentTransactionId must be greater than zero. (Parameter 'paymentTransactionId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_MerchantIdIsInvalid()
    {
        // Arrange
        var merchantId = 0L;

        // Act
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            merchantId,
            ValidRequestPayloadJson,
            ValidRequestHash,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("MerchantId must be greater than zero. (Parameter 'merchantId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_RequestPayloadJsonIsEmpty()
    {
        // Arrange
        var requestPayloadJson = string.Empty;

        // Act
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            requestPayloadJson,
            ValidRequestHash,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("requestPayloadJson is required. (Parameter 'requestPayloadJson')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_RequestHashIsEmpty()
    {
        // Arrange
        var requestHash = string.Empty;

        // Act
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidRequestPayloadJson,
            requestHash,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("requestHash is required. (Parameter 'requestHash')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CorrelationIdIsEmpty()
    {
        // Arrange
        var correlationId = Guid.Empty;

        // Act
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidRequestPayloadJson,
            ValidRequestHash,
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
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidRequestPayloadJson,
            ValidRequestHash,
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
        var act = () => PaymentRequest.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidRequestPayloadJson,
            ValidRequestHash,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }
}
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Domain.UnitTests.Payments;

public class PaymentTransactionStatusHistoryTests
{
    private const long ValidPaymentTransactionId = 1;
    private const string ValidStatusReasonCode = "APPROVED";
    private const string ValidStatusReasonDetail = "Transaction approved successfully";
    private const string ValidAcquirerResponseCode = "00";
    private const string ValidAcquirerResponseMessage = "Approved";
    private static readonly Guid ValidCorrelationId = Guid.NewGuid();

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var paymentTransactionId = ValidPaymentTransactionId;
        var status = PaymentTransactionStatus.Approved;
        var statusReasonCode = ValidStatusReasonCode;
        var statusReasonDetail = ValidStatusReasonDetail;
        var acquirerResponseCode = ValidAcquirerResponseCode;
        var acquirerResponseMessage = ValidAcquirerResponseMessage;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var history = PaymentTransactionStatusHistory.Create(
            paymentTransactionId,
            status,
            statusReasonCode,
            statusReasonDetail,
            acquirerResponseCode,
            acquirerResponseMessage,
            correlationId,
            createdAtUtc,
            createdAtLocal);

        // Assert
        history.PaymentTransactionStatusHistoryId.Should().Be(0); // Default for new entity
        history.PaymentTransactionId.Should().Be(paymentTransactionId);
        history.Status.Should().Be(status);
        history.StatusReasonCode.Should().Be(statusReasonCode);
        history.StatusReasonDetail.Should().Be(statusReasonDetail);
        history.AcquirerResponseCode.Should().Be(acquirerResponseCode);
        history.AcquirerResponseMessage.Should().Be(acquirerResponseMessage);
        history.CorrelationId.Should().Be(correlationId);
        history.CreatedAtUtc.Should().Be(createdAtUtc);
        history.CreatedAtLocal.Should().Be(createdAtLocal);
    }

    [Fact]
    public void Create_Should_TrimAndHandleNullStrings()
    {
        // Arrange
        var paymentTransactionId = ValidPaymentTransactionId;
        var status = PaymentTransactionStatus.Pending;
        string statusReasonCode = "  CODE  ";
        string statusReasonDetail = null!;
        string acquirerResponseCode = "";
        string acquirerResponseMessage = "  MESSAGE  ";
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var history = PaymentTransactionStatusHistory.Create(
            paymentTransactionId,
            status,
            statusReasonCode,
            statusReasonDetail,
            acquirerResponseCode,
            acquirerResponseMessage,
            correlationId,
            createdAtUtc,
            createdAtLocal);

        // Assert
        history.StatusReasonCode.Should().Be("CODE");
        history.StatusReasonDetail.Should().BeEmpty();
        history.AcquirerResponseCode.Should().BeEmpty();
        history.AcquirerResponseMessage.Should().Be("MESSAGE");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_PaymentTransactionIdIsInvalid()
    {
        // Arrange
        var paymentTransactionId = 0L;

        // Act
        var act = () => PaymentTransactionStatusHistory.Create(
            paymentTransactionId,
            PaymentTransactionStatus.Pending,
            ValidStatusReasonCode,
            ValidStatusReasonDetail,
            ValidAcquirerResponseCode,
            ValidAcquirerResponseMessage,
            ValidCorrelationId,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("PaymentTransactionId must be greater than zero. (Parameter 'paymentTransactionId')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CorrelationIdIsEmpty()
    {
        // Arrange
        var correlationId = Guid.Empty;

        // Act
        var act = () => PaymentTransactionStatusHistory.Create(
            ValidPaymentTransactionId,
            PaymentTransactionStatus.Pending,
            ValidStatusReasonCode,
            ValidStatusReasonDetail,
            ValidAcquirerResponseCode,
            ValidAcquirerResponseMessage,
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
        var act = () => PaymentTransactionStatusHistory.Create(
            ValidPaymentTransactionId,
            PaymentTransactionStatus.Pending,
            ValidStatusReasonCode,
            ValidStatusReasonDetail,
            ValidAcquirerResponseCode,
            ValidAcquirerResponseMessage,
            ValidCorrelationId,
            createdAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtUtc is required. (Parameter 'createdAtUtc')");
    }
}
using Haulmer.Payments.Domain.Payments;

namespace Haulmer.Payments.Domain.UnitTests.Payments;

public class PaymentTraceLogTests
{
    private const long ValidPaymentTransactionId = 1;
    private const long ValidMerchantId = 1;
    private const string ValidEventType = "PAYMENT_PROCESSING";
    private const string ValidEventSource = "PaymentService";
    private const string ValidEventDescription = "Payment processing started";
    private const string ValidEventDataJson = "{\"amount\":10000,\"currency\":\"CLP\"}";
    private static readonly Guid ValidCorrelationId = Guid.NewGuid();

    private static DateTime ValidCreatedAtUtc => new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static DateTime ValidCreatedAtLocal => new DateTime(2023, 1, 1, 9, 0, 0, DateTimeKind.Local);

    [Fact]
    public void Create_Should_SetInitialValues_When_InputIsValid()
    {
        // Arrange
        var paymentTransactionId = ValidPaymentTransactionId;
        var merchantId = ValidMerchantId;
        var eventType = ValidEventType;
        var eventSource = ValidEventSource;
        var eventDescription = ValidEventDescription;
        var eventDataJson = ValidEventDataJson;
        var severity = TraceSeverity.Information;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var traceLog = PaymentTraceLog.Create(
            paymentTransactionId,
            merchantId,
            correlationId,
            eventType,
            eventSource,
            eventDescription,
            eventDataJson,
            severity,
            createdAtUtc,
            createdAtLocal);

        // Assert
        traceLog.PaymentTraceLogId.Should().Be(0); // Default for new entity
        traceLog.PaymentTransactionId.Should().Be(paymentTransactionId);
        traceLog.MerchantId.Should().Be(merchantId);
        traceLog.CorrelationId.Should().Be(correlationId);
        traceLog.EventType.Should().Be(eventType);
        traceLog.EventSource.Should().Be(eventSource);
        traceLog.EventDescription.Should().Be(eventDescription);
        traceLog.EventDataJson.Should().Be(eventDataJson);
        traceLog.Severity.Should().Be(severity);
        traceLog.CreatedAtUtc.Should().Be(createdAtUtc);
        traceLog.CreatedAtLocal.Should().Be(createdAtLocal);
    }

    [Fact]
    public void Create_Should_AllowNullPaymentTransactionId()
    {
        // Arrange
        long? paymentTransactionId = null;
        var merchantId = ValidMerchantId;
        var eventType = ValidEventType;
        var eventSource = ValidEventSource;
        var eventDescription = ValidEventDescription;
        var eventDataJson = ValidEventDataJson;
        var severity = TraceSeverity.Information;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var traceLog = PaymentTraceLog.Create(
            paymentTransactionId,
            merchantId,
            correlationId,
            eventType,
            eventSource,
            eventDescription,
            eventDataJson,
            severity,
            createdAtUtc,
            createdAtLocal);

        // Assert
        traceLog.PaymentTransactionId.Should().BeNull();
    }

    [Fact]
    public void Create_Should_AllowNullMerchantId()
    {
        // Arrange
        var paymentTransactionId = ValidPaymentTransactionId;
        long? merchantId = null;
        var eventType = ValidEventType;
        var eventSource = ValidEventSource;
        var eventDescription = ValidEventDescription;
        var eventDataJson = ValidEventDataJson;
        var severity = TraceSeverity.Information;
        var correlationId = ValidCorrelationId;
        var createdAtUtc = ValidCreatedAtUtc;
        var createdAtLocal = ValidCreatedAtLocal;

        // Act
        var traceLog = PaymentTraceLog.Create(
            paymentTransactionId,
            merchantId,
            correlationId,
            eventType,
            eventSource,
            eventDescription,
            eventDataJson,
            severity,
            createdAtUtc,
            createdAtLocal);

        // Assert
        traceLog.MerchantId.Should().BeNull();
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_EventTypeIsEmpty()
    {
        // Arrange
        var eventType = string.Empty;

        // Act
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidCorrelationId,
            eventType,
            ValidEventSource,
            ValidEventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("eventType is required. (Parameter 'eventType')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_EventSourceIsEmpty()
    {
        // Arrange
        var eventSource = string.Empty;

        // Act
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidCorrelationId,
            ValidEventType,
            eventSource,
            ValidEventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("eventSource is required. (Parameter 'eventSource')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_EventDescriptionIsEmpty()
    {
        // Arrange
        var eventDescription = string.Empty;

        // Act
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidCorrelationId,
            ValidEventType,
            ValidEventSource,
            eventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
            ValidCreatedAtUtc,
            ValidCreatedAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("eventDescription is required. (Parameter 'eventDescription')");
    }

    [Fact]
    public void Create_Should_ThrowArgumentException_When_CorrelationIdIsEmpty()
    {
        // Arrange
        var correlationId = Guid.Empty;

        // Act
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            correlationId,
            ValidEventType,
            ValidEventSource,
            ValidEventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
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
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidCorrelationId,
            ValidEventType,
            ValidEventSource,
            ValidEventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
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
        var act = () => PaymentTraceLog.Create(
            ValidPaymentTransactionId,
            ValidMerchantId,
            ValidCorrelationId,
            ValidEventType,
            ValidEventSource,
            ValidEventDescription,
            ValidEventDataJson,
            TraceSeverity.Information,
            ValidCreatedAtUtc,
            createdAtLocal);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("createdAtLocal is required. (Parameter 'createdAtLocal')");
    }
}
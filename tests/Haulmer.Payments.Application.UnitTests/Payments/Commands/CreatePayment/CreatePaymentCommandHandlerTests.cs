using System.Reflection;
using FluentAssertions;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Haulmer.Payments.Application.Payments.Interfaces;
using Haulmer.Payments.Application.Payments.Models;
using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Haulmer.Payments.Domain.Payments;
using Moq;
using Xunit;

namespace Haulmer.Payments.Application.UnitTests.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandlerTests
{
    private readonly Mock<IMerchantRepository> _merchantRepositoryMock;
    private readonly Mock<IMerchantBranchRepository> _merchantBranchRepositoryMock;
    private readonly Mock<IMerchantPaymentMethodRepository> _merchantPaymentMethodRepositoryMock;
    private readonly Mock<IMerchantAcquirerConfigurationRepository> _merchantAcquirerConfigurationRepositoryMock;
    private readonly Mock<IMerchantPricingRepository> _merchantPricingRepositoryMock;
    private readonly Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock;
    private readonly Mock<IPaymentChannelRepository> _paymentChannelRepositoryMock;
    private readonly Mock<IPaymentTransactionRepository> _paymentTransactionRepositoryMock;
    private readonly Mock<IPaymentTransactionStatusHistoryRepository> _paymentTransactionStatusHistoryRepositoryMock;
    private readonly Mock<IPaymentIdempotencyRepository> _paymentIdempotencyRepositoryMock;
    private readonly Mock<IPaymentTraceLogRepository> _paymentTraceLogRepositoryMock;
    private readonly Mock<IPaymentRequestRepository> _paymentRequestRepositoryMock;
    private readonly Mock<IBankTransactionAuthorizationService> _bankAuthorizationServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly CreatePaymentCommandHandler _handler;

    public CreatePaymentCommandHandlerTests()
    {
        _merchantRepositoryMock = new Mock<IMerchantRepository>();
        _merchantBranchRepositoryMock = new Mock<IMerchantBranchRepository>();
        _merchantPaymentMethodRepositoryMock = new Mock<IMerchantPaymentMethodRepository>();
        _merchantAcquirerConfigurationRepositoryMock = new Mock<IMerchantAcquirerConfigurationRepository>();
        _merchantPricingRepositoryMock = new Mock<IMerchantPricingRepository>();
        _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        _paymentChannelRepositoryMock = new Mock<IPaymentChannelRepository>();
        _paymentTransactionRepositoryMock = new Mock<IPaymentTransactionRepository>();
        _paymentTransactionStatusHistoryRepositoryMock = new Mock<IPaymentTransactionStatusHistoryRepository>();
        _paymentIdempotencyRepositoryMock = new Mock<IPaymentIdempotencyRepository>();
        _paymentTraceLogRepositoryMock = new Mock<IPaymentTraceLogRepository>();
        _paymentRequestRepositoryMock = new Mock<IPaymentRequestRepository>();
        _bankAuthorizationServiceMock = new Mock<IBankTransactionAuthorizationService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreatePaymentCommandHandler(
            _merchantRepositoryMock.Object,
            _merchantBranchRepositoryMock.Object,
            _merchantPaymentMethodRepositoryMock.Object,
            _merchantAcquirerConfigurationRepositoryMock.Object,
            _merchantPricingRepositoryMock.Object,
            _paymentMethodRepositoryMock.Object,
            _paymentChannelRepositoryMock.Object,
            _paymentTransactionRepositoryMock.Object,
            _paymentTransactionStatusHistoryRepositoryMock.Object,
            _paymentIdempotencyRepositoryMock.Object,
            _paymentTraceLogRepositoryMock.Object,
            _paymentRequestRepositoryMock.Object,
            _bankAuthorizationServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantNotFound()
    {
        var command = CreateValidCommand();

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Merchant)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Merchant {command.MerchantId} not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantIsInactive()
    {
        var command = CreateValidCommand();
        var merchant = CreateMerchant(1, "MRC001", "12345678-9", MerchantStatus.Inactive, DateTime.UtcNow, DateTime.Now);

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Merchant {command.MerchantId} is not active.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantBranchNotFound()
    {
        var command = CreateValidCommand();
        var merchant = CreateMerchant(1, "MRC001", "12345678-9", MerchantStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        _merchantBranchRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantBranchId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MerchantBranch)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"MerchantBranch {command.MerchantBranchId} not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantBranchDoesNotBelongToMerchant()
    {
        var command = CreateValidCommand();
        var merchant = CreateMerchant(1, "MRC001", "12345678-9", MerchantStatus.Active, DateTime.UtcNow, DateTime.Now);
        var merchantBranch = CreateMerchantBranch(1, 999, "Branch", MerchantBranchStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        _merchantBranchRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantBranchId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantBranch);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"MerchantBranch {command.MerchantBranchId} does not belong to merchant {command.MerchantId}.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantBranchIsInactive()
    {
        var command = CreateValidCommand();
        var merchant = CreateMerchant(1, "MRC001", "12345678-9", MerchantStatus.Active, DateTime.UtcNow, DateTime.Now);
        var merchantBranch = CreateMerchantBranch(1, 1, "Branch", MerchantBranchStatus.Inactive, DateTime.UtcNow, DateTime.Now);

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        _merchantBranchRepositoryMock
            .Setup(x => x.GetByIdAsync(command.MerchantBranchId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantBranch);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"MerchantBranch {command.MerchantBranchId} is not active.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPaymentMethodNotFound()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranch();

        _paymentMethodRepositoryMock
            .Setup(x => x.GetByIdAsync(command.PaymentMethodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentMethod)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"PaymentMethod {command.PaymentMethodId} not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPaymentMethodIsInactive()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranch();

        var paymentMethod = CreatePaymentMethod(1, "CARD", PaymentMethodStatus.Inactive, DateTime.UtcNow, DateTime.Now);

        _paymentMethodRepositoryMock
            .Setup(x => x.GetByIdAsync(command.PaymentMethodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentMethod);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"PaymentMethod {command.PaymentMethodId} is not active.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPaymentChannelNotFound()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranchAndPaymentMethod();

        _paymentChannelRepositoryMock
            .Setup(x => x.GetByIdAsync(command.PaymentChannelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentChannel)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"PaymentChannel {command.PaymentChannelId} not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPaymentChannelIsInactive()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranchAndPaymentMethod();

        var paymentChannel = CreatePaymentChannel(1, "ONLINE", PaymentChannelStatus.Inactive, DateTime.UtcNow, DateTime.Now);

        _paymentChannelRepositoryMock
            .Setup(x => x.GetByIdAsync(command.PaymentChannelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentChannel);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"PaymentChannel {command.PaymentChannelId} is not active.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantPaymentMethodNotFound()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranchAndPaymentMethodAndChannel();

        _merchantPaymentMethodRepositoryMock
            .Setup(x => x.GetByMerchantAndPaymentMethodAsync(command.MerchantId, command.PaymentMethodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MerchantPaymentMethod)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"MerchantPaymentMethod not found for merchant {command.MerchantId} and payment method {command.PaymentMethodId}.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenMerchantPaymentMethodIsInactive()
    {
        var command = CreateValidCommand();
        SetupValidMerchantAndBranchAndPaymentMethodAndChannel();

        var merchantPaymentMethod = CreateMerchantPaymentMethod(
            1, 1, 1, MerchantPaymentMethodStatus.Inactive, DateTime.UtcNow, DateTime.Now);

        _merchantPaymentMethodRepositoryMock
            .Setup(x => x.GetByMerchantAndPaymentMethodAsync(command.MerchantId, command.PaymentMethodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantPaymentMethod);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("MerchantPaymentMethod is not active.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenAcquirerConfigurationNotFound()
    {
        var command = CreateValidCommand();
        SetupValidUpToMerchantPaymentMethod();

        _merchantAcquirerConfigurationRepositoryMock
            .Setup(x => x.GetActiveByMerchantAndPaymentDetailsAsync(
                command.MerchantId,
                command.PaymentMethodId,
                command.PaymentChannelId,
                command.Currency!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((MerchantAcquirerConfiguration)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("MerchantAcquirerConfiguration not found for the specified merchant and payment details.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPricingNotFound()
    {
        var command = CreateValidCommand();
        SetupValidUpToAcquirerConfiguration();

        _merchantPricingRepositoryMock
            .Setup(x => x.GetActiveByMerchantAndPaymentDetailsAsync(
                command.MerchantId,
                command.PaymentMethodId,
                command.PaymentChannelId,
                command.Currency!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((MerchantPricing)null!);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("MerchantPricing not found for the specified merchant and payment details.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenIdempotencyKeyAlreadyExists()
    {
        var command = CreateValidCommand();
        SetupValidUpToPricing();

        var existingIdempotency = CreatePaymentIdempotency(
            1, 1, "test-key", "hash", DateTime.UtcNow, DateTime.Now, null, null);

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.GetByMerchantAndKeyAsync(command.MerchantId, command.IdempotencyKey!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotency);

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"IdempotencyKey {command.IdempotencyKey} already exists for merchant {command.MerchantId}.");
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectResult_WhenApprovedFlow()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        var authorizationResult = new BankTransactionAuthorizationResult
        {
            IsApproved = true,
            AcquirerReference = "REF123",
            ResponseCode = "00",
            ResponseMessage = "Approved",
            IssuingBankName = "Banco Test",
            CardBrand = "VISA",
            CardLast4 = "1234",
            MaskedPan = "**** **** **** 1234"
        };

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorizationResult);

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.PaymentTransactionId.Should().Be(1);
        result.Status.Should().Be(PaymentTransactionStatus.Approved);
        result.AcquirerReference.Should().Be("REF123");

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectResult_WhenDeclinedFlow()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        var authorizationResult = new BankTransactionAuthorizationResult
        {
            IsApproved = false,
            AcquirerReference = "REF456",
            ResponseCode = "05",
            ResponseMessage = "Declined",
            IssuingBankName = "Banco Test",
            CardBrand = "MASTERCARD",
            CardLast4 = "5678",
            MaskedPan = "**** **** **** 5678"
        };

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorizationResult);

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.PaymentTransactionId.Should().Be(1);
        result.Status.Should().Be(PaymentTransactionStatus.Declined);
        result.AcquirerReference.Should().Be("REF456");

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenExternalAuthorizationFails()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Authorization failed"));

        Func<Task> act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Authorization failed");

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldMarkIdempotencyCompleted_WhenApproved()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        var authorizationResult = new BankTransactionAuthorizationResult
        {
            IsApproved = true,
            AcquirerReference = "REF123",
            ResponseCode = "00",
            ResponseMessage = "Approved",
            IssuingBankName = "Banco Test",
            CardBrand = "VISA",
            CardLast4 = "1234",
            MaskedPan = "**** **** **** 1234"
        };

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorizationResult);

        PaymentIdempotency capturedIdempotency = null!;

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentIdempotency>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback<PaymentIdempotency, CancellationToken>((entity, _) => capturedIdempotency = entity);

        await _handler.HandleAsync(command, CancellationToken.None);

        capturedIdempotency.Should().NotBeNull();
        capturedIdempotency.Status.Should().Be(PaymentIdempotencyStatus.Completed);
        capturedIdempotency.PaymentTransactionId.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_ShouldMarkIdempotencyCompleted_WhenDeclined()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        var authorizationResult = new BankTransactionAuthorizationResult
        {
            IsApproved = false,
            AcquirerReference = "REF456",
            ResponseCode = "05",
            ResponseMessage = "Declined",
            IssuingBankName = "Banco Test",
            CardBrand = "MASTERCARD",
            CardLast4 = "5678",
            MaskedPan = "**** **** **** 5678"
        };

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorizationResult);

        PaymentIdempotency capturedIdempotency = null!;

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentIdempotency>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback<PaymentIdempotency, CancellationToken>((entity, _) => capturedIdempotency = entity);

        await _handler.HandleAsync(command, CancellationToken.None);

        capturedIdempotency.Should().NotBeNull();
        capturedIdempotency.Status.Should().Be(PaymentIdempotencyStatus.Completed);
        capturedIdempotency.PaymentTransactionId.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_ShouldMarkIdempotencyFailed_WhenExternalAuthorizationFails()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Authorization failed"));

        PaymentIdempotency capturedIdempotency = null!;

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentIdempotency>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback<PaymentIdempotency, CancellationToken>((entity, _) => capturedIdempotency = entity);

        try
        {
            await _handler.HandleAsync(command, CancellationToken.None);
        }
        catch
        {
            // expected
        }

        capturedIdempotency.Should().NotBeNull();
        capturedIdempotency.Status.Should().Be(PaymentIdempotencyStatus.Failed);
        capturedIdempotency.PaymentTransactionId.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreatePendingAndProcessingAndFinalHistories()
    {
        var command = CreateValidCommand();
        SetupValidUpToIdempotency();
        SetupRepositoriesForSuccess();

        var authorizationResult = new BankTransactionAuthorizationResult
        {
            IsApproved = true,
            AcquirerReference = "REF123",
            ResponseCode = "00",
            ResponseMessage = "Approved",
            IssuingBankName = "Banco Test",
            CardBrand = "VISA",
            CardLast4 = "1234",
            MaskedPan = "**** **** **** 1234"
        };

        _bankAuthorizationServiceMock
            .Setup(x => x.AuthorizeTransactionAsync(It.IsAny<BankTransactionAuthorizationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorizationResult);

        await _handler.HandleAsync(command, CancellationToken.None);

        _paymentTransactionStatusHistoryRepositoryMock.Verify(
            x => x.AddAsync(It.Is<PaymentTransactionStatusHistory>(h => h.Status == PaymentTransactionStatus.Pending), It.IsAny<CancellationToken>()),
            Times.Once);

        _paymentTransactionStatusHistoryRepositoryMock.Verify(
            x => x.AddAsync(It.Is<PaymentTransactionStatusHistory>(h => h.Status == PaymentTransactionStatus.Processing), It.IsAny<CancellationToken>()),
            Times.Once);

        _paymentTransactionStatusHistoryRepositoryMock.Verify(
            x => x.AddAsync(It.Is<PaymentTransactionStatusHistory>(h => h.Status == PaymentTransactionStatus.Approved), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static CreatePaymentCommand CreateValidCommand()
    {
        return new CreatePaymentCommand
        {
            MerchantId = 1,
            MerchantBranchId = 1,
            PaymentMethodId = 1,
            PaymentChannelId = 1,
            IdempotencyKey = "test-key",
            BaseAmount = 100m,
            TipAmount = 10m,
            Currency = "CLP",
            PayerFullName = "John Doe",
            PayerRut = "12345678-9",
            RequestPayloadJson = "{}",
            CorrelationId = Guid.NewGuid()
        };
    }

    private static Merchant CreateMerchant(
        long id,
        string merchantCode,
        string companyRut,
        MerchantStatus status,
        DateTime utc,
        DateTime local)
    {
        var merchant = Merchant.Create(
            merchantCode,
            companyRut,
            "Legal Name",
            "Trade Name",
            "Address",
            "email@test.com",
            "123456789",
            utc,
            local);

        SetPrivateProperty(merchant, nameof(Merchant.MerchantId), id);
        SetPrivateProperty(merchant, nameof(Merchant.Status), status);

        return merchant;
    }

    private static MerchantBranch CreateMerchantBranch(
        long id,
        long merchantId,
        string branchName,
        MerchantBranchStatus status,
        DateTime utc,
        DateTime local)
    {
        var branch = MerchantBranch.Create(
            merchantId,
            "BR001",
            branchName,
            "Address",
            "City",
            "Region",
            "Country",
            utc,
            local);

        SetPrivateProperty(branch, nameof(MerchantBranch.MerchantBranchId), id);
        SetPrivateProperty(branch, nameof(MerchantBranch.Status), status);

        return branch;
    }

    private static PaymentMethod CreatePaymentMethod(
        long id,
        string code,
        PaymentMethodStatus status,
        DateTime utc,
        DateTime local)
    {
        var method = PaymentMethod.Create(code, "Credit Card", utc, local);

        SetPrivateProperty(method, nameof(PaymentMethod.PaymentMethodId), id);
        SetPrivateProperty(method, nameof(PaymentMethod.Status), status);

        return method;
    }

    private static PaymentChannel CreatePaymentChannel(
        long id,
        string code,
        PaymentChannelStatus status,
        DateTime utc,
        DateTime local)
    {
        var channel = PaymentChannel.Create(code, "Online", utc, local);

        SetPrivateProperty(channel, nameof(PaymentChannel.PaymentChannelId), id);
        SetPrivateProperty(channel, nameof(PaymentChannel.Status), status);

        return channel;
    }

    private static MerchantPaymentMethod CreateMerchantPaymentMethod(
        long id,
        long merchantId,
        long paymentMethodId,
        MerchantPaymentMethodStatus status,
        DateTime utc,
        DateTime local)
    {
        var merchantPaymentMethod = MerchantPaymentMethod.Create(
            merchantId,
            paymentMethodId,
            utc,
            local,
            DateTime.UtcNow,
            DateTime.Now);

        SetPrivateProperty(merchantPaymentMethod, nameof(MerchantPaymentMethod.MerchantPaymentMethodId), id);
        SetPrivateProperty(merchantPaymentMethod, nameof(MerchantPaymentMethod.Status), status);

        return merchantPaymentMethod;
    }

    private static MerchantAcquirerConfiguration CreateMerchantAcquirerConfiguration(
        long id,
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        long acquirerId,
        MerchantAcquirerConfigurationStatus status,
        DateTime utc,
        DateTime local)
    {
        var configuration = MerchantAcquirerConfiguration.Create(
            merchantId,
            paymentMethodId,
            paymentChannelId,
            acquirerId,
            "CLP",
            "TERM001",
            "ACQ001",
            30,
            1,
            utc,
            local);

        SetPrivateProperty(configuration, nameof(MerchantAcquirerConfiguration.MerchantAcquirerConfigurationId), id);
        SetPrivateProperty(configuration, nameof(MerchantAcquirerConfiguration.Status), status);

        return configuration;
    }

    private static MerchantPricing CreateMerchantPricing(
        long id,
        long merchantId,
        long paymentMethodId,
        long paymentChannelId,
        MerchantPricingStatus status,
        DateTime utc,
        DateTime local)
    {
        var pricing = MerchantPricing.Create(
            merchantId,
            paymentMethodId,
            paymentChannelId,
            "CLP",
            2.5m,
            1.0m,
            19.0m,
            1.9m,
            0.5m,
            utc,
            local,
            DateTime.UtcNow,
            DateTime.Now);

        SetPrivateProperty(pricing, nameof(MerchantPricing.MerchantPricingId), id);
        SetPrivateProperty(pricing, nameof(MerchantPricing.Status), status);

        return pricing;
    }

    private static PaymentIdempotency CreatePaymentIdempotency(
        long id,
        long merchantId,
        string key,
        string hash,
        DateTime utc,
        DateTime local,
        DateTime? expiresUtc,
        DateTime? expiresLocal)
    {
        var idempotency = PaymentIdempotency.Create(merchantId, key, hash, utc, local, expiresUtc, expiresLocal);
        SetPrivateProperty(idempotency, nameof(PaymentIdempotency.PaymentIdempotencyId), id);
        return idempotency;
    }

    private void SetupValidMerchantAndBranch()
    {
        var merchant = CreateMerchant(1, "MRC001", "12345678-9", MerchantStatus.Active, DateTime.UtcNow, DateTime.Now);
        var merchantBranch = CreateMerchantBranch(1, 1, "Branch", MerchantBranchStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchant);

        _merchantBranchRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantBranch);
    }

    private void SetupValidMerchantAndBranchAndPaymentMethod()
    {
        SetupValidMerchantAndBranch();

        var paymentMethod = CreatePaymentMethod(1, "CARD", PaymentMethodStatus.Active, DateTime.UtcNow, DateTime.Now);

        _paymentMethodRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentMethod);
    }

    private void SetupValidMerchantAndBranchAndPaymentMethodAndChannel()
    {
        SetupValidMerchantAndBranchAndPaymentMethod();

        var paymentChannel = CreatePaymentChannel(1, "ONLINE", PaymentChannelStatus.Active, DateTime.UtcNow, DateTime.Now);

        _paymentChannelRepositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentChannel);
    }

    private void SetupValidUpToMerchantPaymentMethod()
    {
        SetupValidMerchantAndBranchAndPaymentMethodAndChannel();

        var merchantPaymentMethod = CreateMerchantPaymentMethod(
            1, 1, 1, MerchantPaymentMethodStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantPaymentMethodRepositoryMock
            .Setup(x => x.GetByMerchantAndPaymentMethodAsync(1, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchantPaymentMethod);
    }

    private void SetupValidUpToAcquirerConfiguration()
    {
        SetupValidUpToMerchantPaymentMethod();

        var acquirerConfiguration = CreateMerchantAcquirerConfiguration(
            1, 1, 1, 1, 1, MerchantAcquirerConfigurationStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantAcquirerConfigurationRepositoryMock
            .Setup(x => x.GetActiveByMerchantAndPaymentDetailsAsync(1, 1, 1, "CLP", It.IsAny<CancellationToken>()))
            .ReturnsAsync(acquirerConfiguration);
    }

    private void SetupValidUpToPricing()
    {
        SetupValidUpToAcquirerConfiguration();

        var pricing = CreateMerchantPricing(
            1, 1, 1, 1, MerchantPricingStatus.Active, DateTime.UtcNow, DateTime.Now);

        _merchantPricingRepositoryMock
            .Setup(x => x.GetActiveByMerchantAndPaymentDetailsAsync(1, 1, 1, "CLP", It.IsAny<CancellationToken>()))
            .ReturnsAsync(pricing);
    }

    private void SetupValidUpToIdempotency()
    {
        SetupValidUpToPricing();

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.GetByMerchantAndKeyAsync(1, "test-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentIdempotency)null!);
    }

    private void SetupRepositoriesForSuccess()
    {
        _paymentTransactionRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentTransaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback<PaymentTransaction, CancellationToken>((transaction, _) =>
            {
                SetPrivateProperty(transaction, nameof(PaymentTransaction.PaymentTransactionId), 1L);
            });

        _paymentIdempotencyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentIdempotency>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _paymentTransactionStatusHistoryRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentTransactionStatusHistory>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _paymentRequestRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _paymentTraceLogRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<PaymentTraceLog>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    private static void SetPrivateProperty<T>(T target, string propertyName, object value)
    {
        typeof(T)
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(target, value);
    }
}
using FluentValidation.TestHelper;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Xunit;

namespace Haulmer.Payments.Application.UnitTests.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidatorTests
{
    private readonly CreatePaymentCommandValidator _validator;

    public CreatePaymentCommandValidatorTests()
    {
        _validator = new CreatePaymentCommandValidator();
    }

    [Fact]
    public void Should_BeValid_When_CommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveValidationError_When_MerchantIdIsLessThanOrEqualToZero()
    {
        var command = CreateValidCommand();
        command.MerchantId = 0;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.MerchantId)
            .WithErrorMessage("MerchantId must be greater than 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_MerchantBranchIdIsLessThanOrEqualToZero()
    {
        var command = CreateValidCommand();
        command.MerchantBranchId = 0;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.MerchantBranchId)
            .WithErrorMessage("MerchantBranchId must be greater than 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_PaymentMethodIdIsLessThanOrEqualToZero()
    {
        var command = CreateValidCommand();
        command.PaymentMethodId = 0;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId)
            .WithErrorMessage("PaymentMethodId must be greater than 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_PaymentChannelIdIsLessThanOrEqualToZero()
    {
        var command = CreateValidCommand();
        command.PaymentChannelId = 0;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PaymentChannelId)
            .WithErrorMessage("PaymentChannelId must be greater than 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_IdempotencyKeyIsNull()
    {
        var command = CreateValidCommand();
        command.IdempotencyKey = null;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.IdempotencyKey)
            .WithErrorMessage("IdempotencyKey is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_IdempotencyKeyIsWhitespace()
    {
        var command = CreateValidCommand();
        command.IdempotencyKey = "   ";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.IdempotencyKey)
            .WithErrorMessage("IdempotencyKey is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_IdempotencyKeyExceedsMaxLength()
    {
        var command = CreateValidCommand();
        command.IdempotencyKey = new string('A', 101);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.IdempotencyKey)
            .WithErrorMessage("IdempotencyKey must not exceed 100 characters");
    }

    [Fact]
    public void Should_HaveValidationError_When_BaseAmountIsLessThanOrEqualToZero()
    {
        var command = CreateValidCommand();
        command.BaseAmount = 0m;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BaseAmount)
            .WithErrorMessage("BaseAmount must be greater than 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_TipAmountIsLessThanZero()
    {
        var command = CreateValidCommand();
        command.TipAmount = -1m;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.TipAmount)
            .WithErrorMessage("TipAmount must be greater than or equal to 0");
    }

    [Fact]
    public void Should_HaveValidationError_When_CurrencyIsNull()
    {
        var command = CreateValidCommand();
        command.Currency = null;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Currency is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_CurrencyIsWhitespace()
    {
        var command = CreateValidCommand();
        command.Currency = "   ";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Currency is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_CurrencyLengthIsLessThanThree()
    {
        var command = CreateValidCommand();
        command.Currency = "CL";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Currency must be a valid 3-letter ISO code");
    }

    [Fact]
    public void Should_HaveValidationError_When_CurrencyLengthIsGreaterThanThree()
    {
        var command = CreateValidCommand();
        command.Currency = "CLPX";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Currency must be a valid 3-letter ISO code");
    }

    [Fact]
    public void Should_HaveValidationError_When_CurrencyIsNotUppercaseIsoCode()
    {
        var command = CreateValidCommand();
        command.Currency = "clp";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Currency must be a valid 3-letter ISO code");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerFullNameIsNull()
    {
        var command = CreateValidCommand();
        command.PayerFullName = null;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerFullName)
            .WithErrorMessage("PayerFullName is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerFullNameIsWhitespace()
    {
        var command = CreateValidCommand();
        command.PayerFullName = "   ";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerFullName)
            .WithErrorMessage("PayerFullName is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerFullNameExceedsMaxLength()
    {
        var command = CreateValidCommand();
        command.PayerFullName = new string('A', 201);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerFullName)
            .WithErrorMessage("PayerFullName must not exceed 200 characters");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerRutIsNull()
    {
        var command = CreateValidCommand();
        command.PayerRut = null;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerRut)
            .WithErrorMessage("PayerRut is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerRutIsWhitespace()
    {
        var command = CreateValidCommand();
        command.PayerRut = "   ";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerRut)
            .WithErrorMessage("PayerRut is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_PayerRutExceedsMaxLength()
    {
        var command = CreateValidCommand();
        command.PayerRut = "12345678-9012";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PayerRut)
            .WithErrorMessage("PayerRut must not exceed 12 characters");
    }
    [Fact]
    public void Should_HaveValidationError_When_RequestPayloadJsonIsNull()
    {
        var command = CreateValidCommand();
        command.RequestPayloadJson = null;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RequestPayloadJson)
            .WithErrorMessage("RequestPayloadJson is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_RequestPayloadJsonIsWhitespace()
    {
        var command = CreateValidCommand();
        command.RequestPayloadJson = "   ";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RequestPayloadJson)
            .WithErrorMessage("RequestPayloadJson is required");
    }

    [Fact]
    public void Should_HaveValidationError_When_RequestPayloadJsonIsInvalidJson()
    {
        var command = CreateValidCommand();
        command.RequestPayloadJson = "not-json";

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RequestPayloadJson)
            .WithErrorMessage("RequestPayloadJson must be a valid JSON payload");
    }

    [Fact]
    public void Should_NotHaveValidationError_When_RequestPayloadJsonIsValidJsonObject()
    {
        var command = CreateValidCommand();
        command.RequestPayloadJson = "{\"amount\":100,\"currency\":\"CLP\"}";

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.RequestPayloadJson);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_RequestPayloadJsonIsValidJsonArray()
    {
        var command = CreateValidCommand();
        command.RequestPayloadJson = "[{\"amount\":100}]";

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.RequestPayloadJson);
    }

    [Fact]
    public void Should_HaveValidationError_When_CorrelationIdIsEmpty()
    {
        var command = CreateValidCommand();
        command.CorrelationId = Guid.Empty;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CorrelationId)
            .WithErrorMessage("CorrelationId must not be empty");
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
            BaseAmount = 100.00m,
            TipAmount = 10.00m,
            Currency = "CLP",
            PayerFullName = "John Doe",
            PayerRut = "12345678-9",
            RequestPayloadJson = "{}",
            CorrelationId = Guid.NewGuid()
        };
    }
}
using FluentValidation;
using System.Text.Json;

namespace Haulmer.Payments.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.MerchantId)
            .GreaterThan(0)
            .WithMessage("MerchantId must be greater than 0");

        RuleFor(x => x.MerchantBranchId)
            .GreaterThan(0)
            .WithMessage("MerchantBranchId must be greater than 0");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0");

        RuleFor(x => x.PaymentChannelId)
            .GreaterThan(0)
            .WithMessage("PaymentChannelId must be greater than 0");

        RuleFor(x => x.IdempotencyKey)
            .Must(NotBeNullOrWhiteSpace)
            .WithMessage("IdempotencyKey is required")
            .MaximumLength(100)
            .WithMessage("IdempotencyKey must not exceed 100 characters");

        RuleFor(x => x.BaseAmount)
            .GreaterThan(0)
            .WithMessage("BaseAmount must be greater than 0");

        RuleFor(x => x.TipAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("TipAmount must be greater than or equal to 0");

        RuleFor(x => x.Currency)
            .Must(NotBeNullOrWhiteSpace)
            .WithMessage("Currency is required")
            .Length(3)
            .WithMessage("Currency must be a valid 3-letter ISO code")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be a valid 3-letter ISO code");

        RuleFor(x => x.PayerFullName)
            .Must(NotBeNullOrWhiteSpace)
            .WithMessage("PayerFullName is required")
            .MaximumLength(200)
            .WithMessage("PayerFullName must not exceed 200 characters");

        RuleFor(x => x.PayerRut)
            .Must(NotBeNullOrWhiteSpace)
            .WithMessage("PayerRut is required")
            .MaximumLength(12)
            .WithMessage("PayerRut must not exceed 12 characters");
            // TODO: In a future iteration, validate PayerRut using the real Chilean RUT format.

        RuleFor(x => x.RequestPayloadJson)
            .Must(NotBeNullOrWhiteSpace)
            .WithMessage("RequestPayloadJson is required")
            .Must(BeValidJson)
            .WithMessage("RequestPayloadJson must be a valid JSON payload");

        RuleFor(x => x.CorrelationId)
            .NotEqual(Guid.Empty)
            .WithMessage("CorrelationId must not be empty");

    }

    private static bool NotBeNullOrWhiteSpace(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    private static bool BeValidJson(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            using var _ = JsonDocument.Parse(value);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
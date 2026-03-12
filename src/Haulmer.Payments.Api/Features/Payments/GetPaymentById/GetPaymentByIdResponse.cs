namespace Haulmer.Payments.Api.Features.Payments.GetPaymentById;

public record GetPaymentByIdResponse(
	long PaymentTransactionId,
	string TransactionNumber,
	long MerchantId,
	long MerchantBranchId,
	long PaymentMethodId,
	long PaymentChannelId,
	string Currency,
	decimal BaseAmount,
	decimal TipAmount,
	decimal GrossAmount,
	decimal NetAmount,
	string Status,
	string AcquirerReference,
	Guid CorrelationId,
	DateTime CreatedAtUtc,
	DateTime? ProcessedAtUtc
);

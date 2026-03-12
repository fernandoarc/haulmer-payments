namespace Haulmer.Payments.Api.Features.Payments.SearchPayments;

public record SearchPaymentsRequest(
	long? MerchantId,
	string? Status
);

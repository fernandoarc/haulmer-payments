using Microsoft.AspNetCore.Mvc;

namespace Haulmer.Payments.Api.Features.Payments.SearchPayments;

public sealed record SearchPaymentsRequest
{
	[FromQuery(Name = "merchant_id")]
	public long MerchantId { get; init; }

	public string? Status { get; init; }
}

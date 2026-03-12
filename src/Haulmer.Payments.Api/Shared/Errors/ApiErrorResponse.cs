namespace Haulmer.Payments.Api.Shared.Errors;

public class ApiErrorResponse
{
	public int StatusCode { get; set; }
	public string? Code { get; set; }
	public string? Message { get; set; }
	public string? CorrelationId { get; set; }
}

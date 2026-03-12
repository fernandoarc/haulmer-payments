using System.Collections.Generic;

namespace Haulmer.Payments.Api.Shared.Errors;

public class ValidationErrorResponse : ApiErrorResponse
{
	public Dictionary<string, string[]> Errors { get; set; } = new();
}

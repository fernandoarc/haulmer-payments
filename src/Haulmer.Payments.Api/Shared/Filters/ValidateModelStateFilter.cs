using System.Linq;
using Haulmer.Payments.Api.Shared.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Haulmer.Payments.Api.Shared.Filters;

public class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            var correlationId = context.HttpContext.Items.ContainsKey("CorrelationId")
                ? context.HttpContext.Items["CorrelationId"]?.ToString()
                : null;

            var response = new ValidationErrorResponse
            {
                StatusCode = 400,
                Code = "ValidationError",
                Message = "One or more validation errors occurred.",
                CorrelationId = correlationId,
                Errors = errors
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = 400
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No-op
    }
}

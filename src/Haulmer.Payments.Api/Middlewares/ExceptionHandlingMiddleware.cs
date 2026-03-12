using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Haulmer.Payments.Api.Shared.Errors;
using Microsoft.AspNetCore.Http;

namespace Haulmer.Payments.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorResponse(context, ex.Message, 400);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorResponse(context, ex.Message, 400);
        }
        catch (Exception ex)
        {
            await WriteErrorResponse(context, "An unexpected error occurred.", 500, ex);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, string message, int statusCode, Exception? ex = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var correlationId = context.Items.ContainsKey("CorrelationId") ? context.Items["CorrelationId"]?.ToString() : null;

        var error = new ApiErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            CorrelationId = correlationId,
            // Optionally: Exception = ex?.ToString() // For debugging, not for prod
        };

        var json = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(json);
    }
}

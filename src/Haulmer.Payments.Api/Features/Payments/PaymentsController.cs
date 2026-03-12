using FluentValidation;
using Haulmer.Payments.Api.Features.Payments.CreatePayment;
using Haulmer.Payments.Api.Features.Payments.GetPaymentById;
using Haulmer.Payments.Api.Features.Payments.Mappings;
using Haulmer.Payments.Api.Features.Payments.SearchPayments;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Haulmer.Payments.Application.Payments.Queries.GetPaymentById;
using Haulmer.Payments.Application.Payments.Queries.SearchPayments;
using Haulmer.Payments.Domain.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Haulmer.Payments.Api.Features.Payments;

[ApiController]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly CreatePaymentCommandHandler _createPaymentCommandHandler;
    private readonly IValidator<CreatePaymentCommand> _createPaymentCommandValidator;
    private readonly GetPaymentByIdQueryHandler _getPaymentByIdQueryHandler;
    private readonly SearchPaymentsQueryHandler _searchPaymentsQueryHandler;

    public PaymentsController(
        CreatePaymentCommandHandler createPaymentCommandHandler,
        IValidator<CreatePaymentCommand> createPaymentCommandValidator,
        GetPaymentByIdQueryHandler getPaymentByIdQueryHandler,
        SearchPaymentsQueryHandler searchPaymentsQueryHandler)
    {
        _createPaymentCommandHandler = createPaymentCommandHandler;
        _createPaymentCommandValidator = createPaymentCommandValidator;
        _getPaymentByIdQueryHandler = getPaymentByIdQueryHandler;
        _searchPaymentsQueryHandler = searchPaymentsQueryHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatePaymentResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = GetCorrelationId();

        var command = request.ToCreatePaymentCommand(correlationId);

        var validationResult = await _createPaymentCommandValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                Message = "One or more validation errors occurred.",
                Errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray())
            });
        }

        var result = await _createPaymentCommandHandler.HandleAsync(command, cancellationToken);
        var response = result.ToCreatePaymentResponse();

        return CreatedAtAction(
            nameof(GetById),
            new { transactionId = response.PaymentTransactionId },
            response);
    }

    [HttpGet("{transactionId:long}")]
    [ProducesResponseType(typeof(GetPaymentByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long transactionId, CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery
        {
            TransactionId = transactionId
        };

        var result = await _getPaymentByIdQueryHandler.HandleAsync(query, cancellationToken);
        if (result is null)
            return NotFound();

        var response = result.ToGetPaymentByIdResponse();
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchPaymentsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] SearchPaymentsRequest request, CancellationToken cancellationToken)
    {
        if (!TryParseStatus(request.Status, out var parsedStatus))
        {
            return BadRequest(new
            {
                Message = "Invalid status value. Allowed values: Pending, Processing, Approved, Declined, Failed."
            });
        }

        var query = new SearchPaymentsQuery
        {
            MerchantId = request.MerchantId,
            Status = parsedStatus
        };

        try
        {
            var result = await _searchPaymentsQueryHandler.HandleAsync(query, cancellationToken);
            var response = result.ToSearchPaymentsResponse();
            return Ok(response);
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(SearchPaymentsQuery.MerchantId))
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    private static bool TryParseStatus(string? rawStatus, out PaymentTransactionStatus? status)
    {
        status = null;

        if (string.IsNullOrWhiteSpace(rawStatus))
            return true;

        var statusValue = rawStatus.Trim();
        var allowedStatusNames = Enum.GetNames<PaymentTransactionStatus>();

        if (!allowedStatusNames.Contains(statusValue, StringComparer.OrdinalIgnoreCase))
            return false;

        return Enum.TryParse<PaymentTransactionStatus>(statusValue, ignoreCase: true, out var parsedStatus)
            ? (status = parsedStatus) is not null
            : false;
    }

    private Guid GetCorrelationId()
    {
        var rawValue = HttpContext.Items["CorrelationId"]?.ToString();

        return Guid.TryParse(rawValue, out var correlationId)
            ? correlationId
            : Guid.NewGuid();
    }
}
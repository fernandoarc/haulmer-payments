using FluentValidation;
using Haulmer.Payments.Api.Features.Payments.CreatePayment;
using Haulmer.Payments.Api.Features.Payments.Mappings;
using Haulmer.Payments.Application.Payments.Commands.CreatePayment;
using Microsoft.AspNetCore.Mvc;

namespace Haulmer.Payments.Api.Features.Payments;

[ApiController]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly CreatePaymentCommandHandler _createPaymentCommandHandler;
    private readonly IValidator<CreatePaymentCommand> _createPaymentCommandValidator;

    public PaymentsController(
        CreatePaymentCommandHandler createPaymentCommandHandler,
        IValidator<CreatePaymentCommand> createPaymentCommandValidator)
    {
        _createPaymentCommandHandler = createPaymentCommandHandler;
        _createPaymentCommandValidator = createPaymentCommandValidator;
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
    public IActionResult GetById(long transactionId)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpGet]
    public IActionResult Search([FromQuery(Name = "merchant_id")] long? merchantId, [FromQuery] string? status)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    private Guid GetCorrelationId()
    {
        var rawValue = HttpContext.Items["CorrelationId"]?.ToString();

        return Guid.TryParse(rawValue, out var correlationId)
            ? correlationId
            : Guid.NewGuid();
    }
}
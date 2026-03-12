using Haulmer.Payments.Infrastructure.ExternalServices.BankAuthorization;
using Haulmer.Payments.Application.Payments.Models;
using FluentAssertions;
using Xunit;
using System.Threading;

namespace Haulmer.Payments.Infrastructure.IntegrationTests.ExternalServices.BankAuthorization;

public class SimulatedBankTransactionAuthorizationServiceTests
{
    private readonly SimulatedBankTransactionAuthorizationService _service = new();

    [Fact]
    public async Task Should_Return_Approved_When_GrossAmount_Less_Than_Or_Equal_1000000()
    {
        var request = new BankTransactionAuthorizationRequest
        {
            GrossAmount = 1000000,
            TransactionNumber = "TXN-001"
        };

        var result = await _service.AuthorizeTransactionAsync(request, CancellationToken.None);

        result.IsApproved.Should().BeTrue();
        result.ResponseCode.Should().Be("00");
        result.ResponseMessage.Should().Be("Approved");
    }

    [Fact]
    public async Task Should_Return_Declined_When_GrossAmount_Greater_Than_1000000()
    {
        var request = new BankTransactionAuthorizationRequest
        {
            GrossAmount = 1000001,
            TransactionNumber = "TXN-002"
        };

        var result = await _service.AuthorizeTransactionAsync(request, CancellationToken.None);

        result.IsApproved.Should().BeFalse();
        result.ResponseCode.Should().Be("05");
        result.ResponseMessage.Should().Be("Declined");
    }

    [Fact]
    public async Task Should_Generate_Deterministic_AcquirerReference_Based_On_TransactionNumber()
    {
        var request = new BankTransactionAuthorizationRequest
        {
            GrossAmount = 500,
            TransactionNumber = "TXN-ABC"
        };

        var result = await _service.AuthorizeTransactionAsync(request, CancellationToken.None);

        result.AcquirerReference.Should().Be("SIM-TXN-ABC");
    }

    [Fact]
    public async Task Should_Return_Expected_Fixed_Metadata()
    {
        var request = new BankTransactionAuthorizationRequest
        {
            GrossAmount = 500,
            TransactionNumber = "TXN-XYZ"
        };

        var result = await _service.AuthorizeTransactionAsync(request, CancellationToken.None);

        result.IssuingBankName.Should().Be("Banco Simulado");
        result.CardBrand.Should().Be("VISA");
        result.CardLast4.Should().Be("1234");
        result.MaskedPan.Should().Be("****-****-****-1234");
    }
}

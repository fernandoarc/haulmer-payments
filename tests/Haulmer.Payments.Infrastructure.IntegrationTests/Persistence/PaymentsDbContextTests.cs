using Haulmer.Payments.Infrastructure.Persistence;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Payments;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.TestInfrastructure;

namespace Haulmer.Payments.Infrastructure.IntegrationTests.Persistence;

public class PaymentsDbContextTests
{
    [Fact]
    public async Task Should_Enforce_Unique_Index_On_MerchantId_And_IdempotencyKey_For_PaymentIdempotency()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        var context = factory.CreateContext();

        var merchant = Merchant.Create("MID4", "12345678-9", "Empresa Cuatro", "Empresa Cuatro", "Dirección Cuatro", "contacto@cuatro.cl", "444555666", DateTime.UtcNow, DateTime.Now);
        context.Merchants.Add(merchant);
        await context.SaveChangesAsync();

        var idempotency1 = PaymentIdempotency.Create(
            merchant.MerchantId,
            "IDEMP-KEY-3",
            "HASH-3",
            DateTime.UtcNow,
            DateTime.Now,
            null,
            null);
        context.PaymentIdempotencies.Add(idempotency1);
        await context.SaveChangesAsync();

        var idempotency2 = PaymentIdempotency.Create(
            merchant.MerchantId,
            "IDEMP-KEY-3",
            "HASH-4",
            DateTime.UtcNow,
            DateTime.Now,
            null,
            null);
        context.PaymentIdempotencies.Add(idempotency2);

        Func<Task> act = async () => await context.SaveChangesAsync();
        await act.Should().ThrowAsync<DbUpdateException>();
    }
}

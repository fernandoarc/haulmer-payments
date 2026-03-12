using FluentAssertions;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Payments;
using Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.TestInfrastructure;
using Haulmer.Payments.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.Repositories;

public class PaymentIdempotencyRepositoryTests
{
    [Fact]
    public async Task Should_Return_Existing_Idempotency_By_Merchant_And_Key()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        using var context = factory.CreateContext();

        var merchant = Merchant.Create(
            "MID1",
            "12345678-9",
            "Empresa Uno",
            "Empresa Uno",
            "Direccion Uno",
            "contacto@uno.cl",
            "123456789",
            DateTime.UtcNow,
            DateTime.Now);

        context.Merchants.Add(merchant);
        await context.SaveChangesAsync();

        var idempotency = PaymentIdempotency.Create(
            merchant.MerchantId,
            "IDEMP-KEY-1",
            "HASH-1",
            DateTime.UtcNow,
            DateTime.Now,
            null,
            null);

        context.PaymentIdempotencies.Add(idempotency);
        await context.SaveChangesAsync();

        var repo = new PaymentIdempotencyRepository(context);

        var found = await repo.GetByMerchantAndKeyAsync(
            merchant.MerchantId,
            "IDEMP-KEY-1",
            CancellationToken.None);

        found.Should().NotBeNull();
        found!.PaymentIdempotencyId.Should().Be(idempotency.PaymentIdempotencyId);
        found.MerchantId.Should().Be(merchant.MerchantId);
        found.IdempotencyKey.Should().Be("IDEMP-KEY-1");
    }

    [Fact]
    public async Task Should_Return_Null_When_Idempotency_Does_Not_Exist()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        using var context = factory.CreateContext();

        var merchant = Merchant.Create(
            "MID2",
            "12345678-9",
            "Empresa Dos",
            "Empresa Dos",
            "Direccion Dos",
            "contacto@dos.cl",
            "987654321",
            DateTime.UtcNow,
            DateTime.Now);

        context.Merchants.Add(merchant);
        await context.SaveChangesAsync();

        var repo = new PaymentIdempotencyRepository(context);

        var found = await repo.GetByMerchantAndKeyAsync(
            merchant.MerchantId,
            "NON-EXISTENT",
            CancellationToken.None);

        found.Should().BeNull();
    }

    [Fact]
    public async Task Should_Persist_New_Idempotency_With_AddAsync_And_SaveChangesAsync()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        using var context = factory.CreateContext();

        var merchant = Merchant.Create(
            "MID3",
            "12345678-9",
            "Empresa Tres",
            "Empresa Tres",
            "Direccion Tres",
            "contacto@tres.cl",
            "111222333",
            DateTime.UtcNow,
            DateTime.Now);

        context.Merchants.Add(merchant);
        await context.SaveChangesAsync();

        var repo = new PaymentIdempotencyRepository(context);

        var idempotency = PaymentIdempotency.Create(
            merchant.MerchantId,
            "IDEMP-KEY-2",
            "HASH-2",
            DateTime.UtcNow,
            DateTime.Now,
            null,
            null);

        await repo.AddAsync(idempotency, CancellationToken.None);
        await context.SaveChangesAsync();

        var found = await context.PaymentIdempotencies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.MerchantId == merchant.MerchantId && x.IdempotencyKey == "IDEMP-KEY-2",
                CancellationToken.None);

        found.Should().NotBeNull();
        found!.PaymentIdempotencyId.Should().Be(idempotency.PaymentIdempotencyId);
        found.MerchantId.Should().Be(merchant.MerchantId);
        found.Status.Should().Be(PaymentIdempotencyStatus.InProgress);
    }
}
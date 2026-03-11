using Haulmer.Payments.Infrastructure.Persistence.Repositories;
using Haulmer.Payments.Infrastructure.Persistence;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Catalogs;
// ...existing code...
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;
using Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.TestInfrastructure;

namespace Haulmer.Payments.Infrastructure.IntegrationTests.Persistence.Repositories;

public class MerchantAcquirerConfigurationRepositoryTests
{
    [Fact]
    public async Task Should_Return_Active_Configuration_Matching_Details()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        var context = factory.CreateContext();

        var merchant = Merchant.Create("M001", "12345678-9", "Empresa Uno", "Empresa Uno", "Dirección Uno", "contacto@uno.cl", "123456789", DateTime.UtcNow, DateTime.Now);
        var paymentMethod = PaymentMethod.Create("PM001", "Tarjeta Crédito", DateTime.UtcNow, DateTime.Now);
        var paymentChannel = PaymentChannel.Create("PC001", "Web", DateTime.UtcNow, DateTime.Now);
        var acquirer = Acquirer.Create("ACQ001", "Banco X", DateTime.UtcNow, DateTime.Now);
        context.Merchants.Add(merchant);
        context.PaymentMethods.Add(paymentMethod);
        context.PaymentChannels.Add(paymentChannel);
        context.Acquirers.Add(acquirer);
        await context.SaveChangesAsync();

        var config = MerchantAcquirerConfiguration.Create(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            acquirer.AcquirerId,
            "CLP",
            "TERM001",
            "ACQCODE001",
            30,
            1,
            DateTime.UtcNow,
            DateTime.Now);
        context.MerchantAcquirerConfigurations.Add(config);
        await context.SaveChangesAsync();

        var repo = new MerchantAcquirerConfigurationRepository(context);
        var result = await repo.GetActiveByMerchantAndPaymentDetailsAsync(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            "CLP",
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.MerchantAcquirerConfigurationId.Should().Be(config.MerchantAcquirerConfigurationId);
    }

    [Fact]
    public async Task Should_Return_Configuration_With_Lowest_Priority_When_Multiple_Active()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        var context = factory.CreateContext();

        var merchant = Merchant.Create("M002", "12345678-9", "Empresa Dos", "Empresa Dos", "Dirección Dos", "contacto@dos.cl", "987654321", DateTime.UtcNow, DateTime.Now);
        var paymentMethod = PaymentMethod.Create("PM002", "Tarjeta Débito", DateTime.UtcNow, DateTime.Now);
        var paymentChannel = PaymentChannel.Create("PC002", "Móvil", DateTime.UtcNow, DateTime.Now);
        var acquirer = Acquirer.Create("ACQ002", "Banco Y", DateTime.UtcNow, DateTime.Now);
        context.Merchants.Add(merchant);
        context.PaymentMethods.Add(paymentMethod);
        context.PaymentChannels.Add(paymentChannel);
        context.Acquirers.Add(acquirer);
        await context.SaveChangesAsync();

        var config1 = MerchantAcquirerConfiguration.Create(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            acquirer.AcquirerId,
            "CLP",
            "TERM002",
            "ACQCODE002",
            30,
            2,
            DateTime.UtcNow,
            DateTime.Now);
        var config2 = MerchantAcquirerConfiguration.Create(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            acquirer.AcquirerId,
            "CLP",
            "TERM003",
            "ACQCODE003",
            30,
            1,
            DateTime.UtcNow,
            DateTime.Now);
        context.MerchantAcquirerConfigurations.AddRange(config1, config2);
        await context.SaveChangesAsync();

        var repo = new MerchantAcquirerConfigurationRepository(context);
        var result = await repo.GetActiveByMerchantAndPaymentDetailsAsync(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            "CLP",
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.Priority.Should().Be(1);
        result.MerchantAcquirerConfigurationId.Should().Be(config2.MerchantAcquirerConfigurationId);
    }

    [Fact]
    public async Task Should_Return_Null_When_No_Active_Configuration_Matches()
    {
        using var factory = new SqliteInMemoryDbContextFactory();
        var context = factory.CreateContext();

        var merchant = Merchant.Create("M003", "12345678-9", "Empresa Tres", "Empresa Tres", "Dirección Tres", "contacto@tres.cl", "111222333", DateTime.UtcNow, DateTime.Now);
        var paymentMethod = PaymentMethod.Create("PM003", "Tarjeta Prepago", DateTime.UtcNow, DateTime.Now);
        var paymentChannel = PaymentChannel.Create("PC003", "POS", DateTime.UtcNow, DateTime.Now);
        var acquirer = Acquirer.Create("ACQ003", "Banco Z", DateTime.UtcNow, DateTime.Now);
        context.Merchants.Add(merchant);
        context.PaymentMethods.Add(paymentMethod);
        context.PaymentChannels.Add(paymentChannel);
        context.Acquirers.Add(acquirer);
        await context.SaveChangesAsync();

        var config = MerchantAcquirerConfiguration.Create(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            acquirer.AcquirerId,
            "CLP",
            "TERM004",
            "ACQCODE004",
            30,
            1,
            DateTime.UtcNow,
            DateTime.Now);
        config.Deactivate(DateTime.UtcNow, DateTime.Now);
        context.MerchantAcquirerConfigurations.Add(config);
        await context.SaveChangesAsync();

        var repo = new MerchantAcquirerConfigurationRepository(context);
        var result = await repo.GetActiveByMerchantAndPaymentDetailsAsync(
            merchant.MerchantId,
            paymentMethod.PaymentMethodId,
            paymentChannel.PaymentChannelId,
            "CLP",
            CancellationToken.None);

        result.Should().BeNull();
    }
}

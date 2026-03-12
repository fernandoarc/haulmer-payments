using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Haulmer.Payments.Infrastructure.Persistence;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    public DbSet<Merchant> Merchants { get; set; } = null!;
    public DbSet<MerchantBranch> MerchantBranches { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
    public DbSet<PaymentChannel> PaymentChannels { get; set; } = null!;
    public DbSet<Acquirer> Acquirers { get; set; } = null!;
    public DbSet<MerchantPaymentMethod> MerchantPaymentMethods { get; set; } = null!;
    public DbSet<MerchantAcquirerConfiguration> MerchantAcquirerConfigurations { get; set; } = null!;
    public DbSet<MerchantPricing> MerchantPricings { get; set; } = null!;
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
    public DbSet<PaymentTransactionStatusHistory> PaymentTransactionStatusHistories { get; set; } = null!;
    public DbSet<PaymentIdempotency> PaymentIdempotencies { get; set; } = null!;
    public DbSet<PaymentTraceLog> PaymentTraceLogs { get; set; } = null!;
    public DbSet<PaymentRequest> PaymentRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsDbContext).Assembly);
    }
}
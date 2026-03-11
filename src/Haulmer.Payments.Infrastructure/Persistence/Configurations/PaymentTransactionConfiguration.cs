using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions", "payments");

        builder.HasKey(x => x.PaymentTransactionId);

        builder.Property(x => x.PaymentTransactionId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.TransactionNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.MerchantBranchId)
            .IsRequired();

        builder.Property(x => x.PaymentMethodId)
            .IsRequired();

        builder.Property(x => x.PaymentChannelId)
            .IsRequired();

        builder.Property(x => x.MerchantPaymentMethodId)
            .IsRequired();

        builder.Property(x => x.MerchantAcquirerConfigurationId)
            .IsRequired();

        builder.Property(x => x.AcquirerId)
            .IsRequired();

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.BaseAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TipAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.GrossAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.FeeAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.VatAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TaxAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.OtherChargesAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.NetAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PayerFullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PayerRut)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(x => x.IssuingBankName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CardBrand)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CardLast4)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(x => x.MaskedPan)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(x => x.CurrentStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.AcquirerReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CorrelationId)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.CreatedAtLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.UpdatedAtLocal)
            .HasColumnType("datetime2");

        builder.Property(x => x.ProcessedAtUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.ProcessedAtLocal)
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.TransactionNumber)
            .IsUnique();

        builder.HasIndex(x => x.MerchantId);

        builder.HasIndex(x => x.CurrentStatus);

        builder.HasIndex(x => x.CorrelationId);

        builder.HasIndex(x => new { x.MerchantId, x.CurrentStatus });

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MerchantBranch>()
            .WithMany()
            .HasForeignKey(x => x.MerchantBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentChannel>()
            .WithMany()
            .HasForeignKey(x => x.PaymentChannelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MerchantPaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.MerchantPaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MerchantAcquirerConfiguration>()
            .WithMany()
            .HasForeignKey(x => x.MerchantAcquirerConfigurationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Acquirer>()
            .WithMany()
            .HasForeignKey(x => x.AcquirerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
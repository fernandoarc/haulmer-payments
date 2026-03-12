using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class MerchantPricingConfiguration : IEntityTypeConfiguration<MerchantPricing>
{
    public void Configure(EntityTypeBuilder<MerchantPricing> builder)
    {
        builder.ToTable("MerchantPricings", "payments");

        builder.HasKey(x => x.MerchantPricingId);

        builder.Property(x => x.MerchantPricingId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.PaymentMethodId)
            .IsRequired();

        builder.Property(x => x.PaymentChannelId)
            .IsRequired();

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.FixedFeeAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.VariableFeePercentage)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(x => x.VatPercentage)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(x => x.TaxPercentage)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(x => x.OtherChargePercentage)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(x => x.ValidFromUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.ValidFromLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.ValidToUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.ValidToLocal)
            .HasColumnType("datetime2");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

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

        builder.HasIndex(x => new { x.MerchantId, x.PaymentMethodId, x.PaymentChannelId, x.Currency, x.Status });

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentChannel>()
            .WithMany()
            .HasForeignKey(x => x.PaymentChannelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class MerchantPaymentMethodConfiguration : IEntityTypeConfiguration<MerchantPaymentMethod>
{
    public void Configure(EntityTypeBuilder<MerchantPaymentMethod> builder)
    {
        builder.ToTable("MerchantPaymentMethods", "payments");

        builder.HasKey(x => x.MerchantPaymentMethodId);

        builder.Property(x => x.MerchantPaymentMethodId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.PaymentMethodId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.EnabledFromUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.EnabledFromLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.EnabledToUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.EnabledToLocal)
            .HasColumnType("datetime2");

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

        builder.HasIndex(x => new { x.MerchantId, x.PaymentMethodId });

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
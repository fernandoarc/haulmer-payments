using Haulmer.Payments.Domain.Catalogs;
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.MerchantSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class MerchantAcquirerConfigurationMap : IEntityTypeConfiguration<MerchantAcquirerConfiguration>
{
    public void Configure(EntityTypeBuilder<MerchantAcquirerConfiguration> builder)
    {
        builder.ToTable("MerchantAcquirerConfigurations", "payments");

        builder.HasKey(x => x.MerchantAcquirerConfigurationId);

        builder.Property(x => x.MerchantAcquirerConfigurationId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.PaymentMethodId)
            .IsRequired();

        builder.Property(x => x.PaymentChannelId)
            .IsRequired();

        builder.Property(x => x.AcquirerId)
            .IsRequired();

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.MerchantTerminalCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.AcquirerMerchantCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TimeoutSeconds)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

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

        builder.HasOne<Acquirer>()
            .WithMany()
            .HasForeignKey(x => x.AcquirerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
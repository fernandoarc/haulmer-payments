using Haulmer.Payments.Domain.Merchants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants", "payments");

        builder.HasKey(x => x.MerchantId);

        builder.Property(x => x.MerchantId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CompanyRut)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(x => x.BusinessLegalName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.TradeName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.HeadOfficeAddress)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.ContactEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContactPhone)
            .IsRequired()
            .HasMaxLength(30);

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

        builder.HasIndex(x => x.MerchantCode)
            .IsUnique();

        builder.HasIndex(x => x.CompanyRut)
            .IsUnique();

        builder.HasIndex(x => x.Status);
    }
}

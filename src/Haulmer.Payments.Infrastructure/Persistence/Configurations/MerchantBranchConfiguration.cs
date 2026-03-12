using Haulmer.Payments.Domain.Merchants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class MerchantBranchConfiguration : IEntityTypeConfiguration<MerchantBranch>
{
    public void Configure(EntityTypeBuilder<MerchantBranch> builder)
    {
        builder.ToTable("MerchantBranches", "payments");

        builder.HasKey(x => x.MerchantBranchId);

        builder.Property(x => x.MerchantBranchId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.BranchCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Region)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

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

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.MerchantId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => new { x.MerchantId, x.BranchCode }).IsUnique();
    }
}

using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class PaymentIdempotencyConfiguration : IEntityTypeConfiguration<PaymentIdempotency>
{
    public void Configure(EntityTypeBuilder<PaymentIdempotency> builder)
    {
        builder.ToTable("PaymentIdempotencies", "payments");

        builder.HasKey(x => x.PaymentIdempotencyId);

        builder.Property(x => x.PaymentIdempotencyId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.RequestHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PaymentTransactionId);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.CreatedAtLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.ExpiresAtUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.ExpiresAtLocal)
            .HasColumnType("datetime2");

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnType("datetime2");

        builder.Property(x => x.UpdatedAtLocal)
            .HasColumnType("datetime2");

        builder.HasIndex(x => new { x.MerchantId, x.IdempotencyKey })
            .IsUnique();

        builder.HasIndex(x => x.PaymentTransactionId);

        builder.HasIndex(x => x.Status);

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentTransaction>()
            .WithMany()
            .HasForeignKey(x => x.PaymentTransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
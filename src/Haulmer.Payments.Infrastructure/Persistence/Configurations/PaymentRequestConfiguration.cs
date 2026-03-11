using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class PaymentRequestConfiguration : IEntityTypeConfiguration<PaymentRequest>
{
    public void Configure(EntityTypeBuilder<PaymentRequest> builder)
    {
        builder.ToTable("PaymentRequests", "payments");

        builder.HasKey(x => x.PaymentRequestId);

        builder.Property(x => x.PaymentRequestId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PaymentTransactionId)
            .IsRequired();

        builder.Property(x => x.MerchantId)
            .IsRequired();

        builder.Property(x => x.RequestPayloadJson)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.RequestHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CorrelationId)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.CreatedAtLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.PaymentTransactionId);

        builder.HasIndex(x => x.MerchantId);

        builder.HasIndex(x => x.CorrelationId);

        builder.HasOne<PaymentTransaction>()
            .WithMany()
            .HasForeignKey(x => x.PaymentTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Merchant>()
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using Haulmer.Payments.Domain.Merchants;
using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class PaymentTraceLogConfiguration : IEntityTypeConfiguration<PaymentTraceLog>
{
    public void Configure(EntityTypeBuilder<PaymentTraceLog> builder)
    {
        builder.ToTable("PaymentTraceLogs", "payments");

        builder.HasKey(x => x.PaymentTraceLogId);

        builder.Property(x => x.PaymentTraceLogId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PaymentTransactionId);

        builder.Property(x => x.MerchantId);

        builder.Property(x => x.CorrelationId)
            .IsRequired();

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EventSource)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.EventDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.EventDataJson);

        builder.Property(x => x.Severity)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.CreatedAtLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.PaymentTransactionId);

        builder.HasIndex(x => x.MerchantId);

        builder.HasIndex(x => x.CorrelationId);

        builder.HasIndex(x => x.CreatedAtUtc);

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
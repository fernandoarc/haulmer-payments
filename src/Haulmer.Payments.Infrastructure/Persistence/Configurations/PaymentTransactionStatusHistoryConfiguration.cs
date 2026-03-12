using Haulmer.Payments.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haulmer.Payments.Infrastructure.Persistence.Configurations;

public class PaymentTransactionStatusHistoryConfiguration : IEntityTypeConfiguration<PaymentTransactionStatusHistory>
{
    public void Configure(EntityTypeBuilder<PaymentTransactionStatusHistory> builder)
    {
        builder.ToTable("PaymentTransactionStatusHistories", "payments");

        builder.HasKey(x => x.PaymentTransactionStatusHistoryId);

        builder.Property(x => x.PaymentTransactionStatusHistoryId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PaymentTransactionId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.StatusReasonCode)
            .HasMaxLength(100);

        builder.Property(x => x.StatusReasonDetail)
            .HasMaxLength(500);

        builder.Property(x => x.AcquirerResponseCode)
            .HasMaxLength(100);

        builder.Property(x => x.AcquirerResponseMessage)
            .HasMaxLength(500);

        builder.Property(x => x.CorrelationId)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.CreatedAtLocal)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.PaymentTransactionId);

        builder.HasIndex(x => new { x.PaymentTransactionId, x.CreatedAtUtc });

        builder.HasOne<PaymentTransaction>()
            .WithMany()
            .HasForeignKey(x => x.PaymentTransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
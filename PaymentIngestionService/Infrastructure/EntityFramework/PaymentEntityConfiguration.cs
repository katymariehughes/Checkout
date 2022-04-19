using Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentIngestionService.Domain;

namespace PaymentIngestionService.Infrastructure.EntityFramework
{
    public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.CardDetails, cd =>
            {
                cd.Property(d => d.CardNumber).HasColumnName(nameof(CardDetails.CardNumber)).IsRequired();
                cd.OwnsOne(d => d.ExpiryDate, ex =>
                {
                    ex.Property(e => e.Month).HasColumnName("ExpiryMonth").IsRequired();
                    ex.Property(e => e.Year).HasColumnName("ExpiryYear").IsRequired();
                });
                cd.Property(d => d.CVV).HasColumnName(nameof(CardDetails.CVV)).IsRequired();
            });

            builder.OwnsOne(x => x.Amount, am =>
            {
                am.Property(a => a.Currency).IsRequired();
                am.Property(a => a.Value).IsRequired();
            });

            builder.Property<DateTime>("CreatedOn").IsRequired().HasDefaultValueSql("getutcdate()").HasUtcConversion();
        }
    }
}

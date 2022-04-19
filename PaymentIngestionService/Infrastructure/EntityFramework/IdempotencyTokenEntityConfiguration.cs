using Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentIngestionService.Infrastructure.EntityFramework
{
    public class IdempotencyTokenEntityConfiguration : IEntityTypeConfiguration<IdempotencyToken>
    {
        public void Configure(EntityTypeBuilder<IdempotencyToken> builder)
        {
            // Unique constraint will be applied, meaning we won't insert a duplicate message acknowledgement
            builder.HasKey(x => new { x.MessageId, x.Consumer });

            builder.Property<DateTime>("CreatedOn").IsRequired().HasDefaultValueSql("getutcdate()").HasUtcConversion();
        }
    }
}

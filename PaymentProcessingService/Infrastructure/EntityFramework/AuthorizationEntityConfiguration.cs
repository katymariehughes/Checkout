using Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentProcessingService.Domain;

namespace PaymentProcessingService.Infrastructure.EntityFramework
{
    public class AuthorizationEntityConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PaymentId).IsRequired();
            builder.Property(x => x.Approved).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ResponseCode).IsRequired();
            builder.Property(x => x.ResponseSummary).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Scheme).IsRequired();
            builder.Property(x => x.Last4).IsRequired().IsFixedLength().HasMaxLength(4);
            builder.Property(x => x.Bin).IsRequired();
            builder.Property(x => x.CardType).IsRequired();
            builder.Property(x => x.Issuer).IsRequired();
            builder.Property(x => x.IssuerCountry).IsRequired();

            builder.Property<DateTime>("CreatedOn").IsRequired().HasDefaultValueSql("getutcdate()").HasUtcConversion();
        }
    }
}

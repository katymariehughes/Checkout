using Common.Database;
using Microsoft.EntityFrameworkCore;
using PaymentIngestionService.Domain;
using System.Reflection;

namespace PaymentIngestionService.Infrastructure.EntityFramework
{
    public class IngestionContext : DbContext
    {
        public IngestionContext(DbContextOptions<IngestionContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IdempotencyTokenEntityConfiguration)));
        }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<IdempotencyToken> IdempotencyTokens { get; set; }

        public async Task PersistIdemptencyToken(Guid messageId, string consumerName)
        {
            IdempotencyTokens.Add(new IdempotencyToken
            {
                MessageId = messageId,
                Consumer = consumerName
            });
            await SaveChangesAsync();
        }

        public async Task<bool> HasBeenProcessed(Guid messageId, string consumerName)
        {
            return await IdempotencyTokens.AnyAsync(x => x.MessageId == messageId && x.Consumer == consumerName);
        }
    }
}

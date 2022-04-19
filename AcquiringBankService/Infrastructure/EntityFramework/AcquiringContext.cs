using Common.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AcquiringBankService.Infrastructure.EntityFramework
{
    public class AcquiringContext : DbContext
    {
        public AcquiringContext(DbContextOptions<AcquiringContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IdempotencyTokenEntityConfiguration)));
        }

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

using Common.Database;
using Microsoft.EntityFrameworkCore;
using PaymentProcessingService.Domain;
using System.Reflection;

namespace PaymentProcessingService.Infrastructure.EntityFramework
{
    public class ProcessorContext : DbContext
    {
        public ProcessorContext(DbContextOptions<ProcessorContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IdempotencyTokenEntityConfiguration)));
        }

        public DbSet<IdempotencyToken> IdempotencyTokens { get; set; }

        public DbSet<Authorization> Authorizations { get; set; }

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

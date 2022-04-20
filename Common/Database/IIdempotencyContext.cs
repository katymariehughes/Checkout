using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Common.Database
{
    public interface IIdempotencyContext
    {
        DatabaseFacade Database { get; }

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        Task<bool> HasBeenProcessed(Guid messageId, string consumerName);
        Task PersistIdemptencyToken(Guid messageId, string consumerName);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
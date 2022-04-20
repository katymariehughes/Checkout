using Common.Database;

namespace PaymentIngestionService.Infrastructure.EntityFramework
{
    public interface IIngestionContext : IIdempotencyContext
    {
    }
}
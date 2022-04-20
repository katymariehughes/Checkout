using Common.Database;

namespace PaymentProcessingService.Infrastructure.EntityFramework
{
    public interface IProcessorContext : IIdempotencyContext
    {
    }
}

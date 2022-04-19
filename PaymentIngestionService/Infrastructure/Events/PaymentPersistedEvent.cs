using Common.Events;

namespace PaymentIngestionService.Infrastructure.Events
{
    public class PaymentPersistedEvent
    {
        public Guid PaymentId { get; set; }

        public CardDetails CardDetails { get; init; }

        public Amount Amount { get; init; }
    }
}

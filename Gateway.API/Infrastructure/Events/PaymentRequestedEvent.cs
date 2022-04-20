using Common.Events;
using Gateway.API.Models;

namespace Gateway.API.Infrastructure.Events
{
    public class PaymentRequestedEvent
    {
        public Guid MerchantId { get; set; }

        public CardDetails CardDetails { get; init; }

        public Amount Amount { get; init; }
    }
}

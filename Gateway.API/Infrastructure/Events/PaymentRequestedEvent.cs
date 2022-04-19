using Common.Events;
using Gateway.API.Models;

namespace Gateway.API.Infrastructure.Events
{
    public class PaymentRequestedEvent : IEvent
    {
        public CardDetails CardDetails { get; init; }

        public Amount Amount { get; init; }
    }
}

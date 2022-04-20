using Common.Events;

namespace PaymentIngestionService.Infrastructure.Events
{
    public class PaymentRequestedEvent
    {
        public Guid MerchantId { get; set; }

        public CardDetails CardDetails { get; init; }

        public Amount Amount { get; init; }
    }

    public class CardDetails
    {
        public string CardNumber { get; init; }

        public ExpiryDate ExpiryDate { get; init; }

        public string CVV { get; init; }
    }

    public record ExpiryDate
    {
        public int Month { get; init; }

        public int Year { get; init; }
    }

    public class Amount
    {
        public string Currency { get; init; }

        public int Value { get; init; }
    }
}

namespace Gateway.API.Models
{
    public class PaymentRequest
    {
        public CardDetails CardDetails { get; init; }

        public Amount Amount { get; init; }
    }

    public class CardDetails
    {
        public string CardNumber { get; set; }

        public ExpiryDate ExpiryDate { get; init; }

        public string CVV { get; set; }
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

namespace PaymentIngestionService.Domain
{
    public class Payment
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }

        public CardDetails CardDetails { get; set; }

        public Amount Amount { get; set; }
    }

    public class CardDetails
    {
        public string CardNumber { get; set; }

        public ExpiryDate ExpiryDate { get; set; }

        public string CVV { get; set; }
    }

    public record ExpiryDate
    {
        public int Month { get; set; }

        public int Year { get; set; }
    }

    public class Amount
    {
        public string Currency { get; set; }

        public int Value { get; set; }
    }
}

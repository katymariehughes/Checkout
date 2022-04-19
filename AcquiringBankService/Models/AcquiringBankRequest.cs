namespace AcquiringBankService.Models
{
    public class AcquiringBankRequest
    {
        public string CardNumber { get; init; }

        public int ExpiryMonth { get; init; }

        public int ExpiryYear { get; init; }

        public string CVV { get; init; }

        public string Currency { get; init; }

        public int Amount { get; init; }
    }
}

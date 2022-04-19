namespace Gateway.API.Models
{
    public class PaymentDetails
    {
        public Guid PaymentId { get; init; }
        public string Currency { get; init; }
        public int Amount { get; init; }
        public bool Approved { get; init; }
        public string Status { get; init; }
        public string ResponseCode { get; init; }
        public string ResponseSummary { get; init; }
        public string Type { get; init; }
        public string Scheme { get; init; }
        public int ExpiryMonth { get; init; }
        public int ExpiryYear { get; init; }
        public string Last4 { get; init; }
        public string Bin { get; init; }
        public string CardType { get; init; }
        public string Issuer { get; init; }
        public string IssuerCountry { get; init; }
    }
}

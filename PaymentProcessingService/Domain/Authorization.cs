namespace PaymentProcessingService.Domain
{
    public class Authorization
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public bool Approved { get; init; }
        public string Status { get; init; }
        public string ResponseCode { get; init; }
        public string ResponseSummary { get; init; }
        public string Type { get; init; }
        public string Scheme { get; init; }
        public string Last4 { get; init; }
        public string Bin { get; init; }
        public string CardType { get; init; }
        public string Issuer { get; init; }
        public string IssuerCountry { get; init; }
    }
}

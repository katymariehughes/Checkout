public class ProcessPaymentResponse
{
    public Guid ResponseId { get; init; }
    public int Amount { get; init; }
    public string Currency { get; init; }
    public bool Approved { get; init; }
    public string Status { get; init; }
    public string ResponseCode { get; init; }
    public string ResponseSummary { get; init; }
    public Source Source { get; init; }
    public DateTime ProcessedOn { get; init; }

    public static ProcessPaymentResponse Generate(ProcessPaymentRequest request)
    {
        return new ProcessPaymentResponse
        {
            ResponseId = Guid.NewGuid(),
            Amount = request.Amount,
            Currency = request.Currency,
            Approved = Random(true, false),
            Status = Random("Approved", "Pending", "Rejected", "Invalid"),
            ResponseCode = Random("252542", "3542", "32564", "45543"),
            ResponseSummary = Random("Authorized", "Pending"),
            Source = new Source
            {
                Type = "Card",
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Scheme = Random("Visa", "Mastercard"),
                Last4 = string.Concat(request.CardNumber.TakeLast(4)),
                Bin = "424242",
                CardType = Random("Credit", "Debit"),
                Issuer = Random("HSBC", "Monzo", "Lloyds", "Starling"),
                IssuerCountry = Random("GB", "US", "FR", "DA")
            }
        };
    }

    private static T Random<T>(params T[] options)
    {
        int index = new Random().Next(options.Length);
        return options[index];
    }
}

public class Source
{
    public string Type { get; init; }
    public int ExpiryMonth { get; init; }
    public int ExpiryYear { get; init; }
    public string Scheme { get; init; }
    public string Last4 { get; init; }
    public string Bin { get; init; }
    public string CardType { get; init; }
    public string Issuer { get; init; }
    public string IssuerCountry { get; init; }
}
public class ProcessPaymentRequest
{
    public string CardNumber { get; set; }

    public int ExpiryMonth { get; init; }

    public int ExpiryYear { get; init; }

    public string CVV { get; set; }

    public string Currency { get; init; }

    public int Amount { get; init; }
}
﻿public class ProcessPaymentRequest
{
    public string CardNumber { get; set; }

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string CVV { get; set; }

    public string Currency { get; set; }

    public int Amount { get; set; }
}
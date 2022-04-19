using Gateway.API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Gateway.API.Infrastructure.Swagger
{
    public class PaymentRequestExample : IExamplesProvider<PaymentRequest>
    {
        public PaymentRequest GetExamples()
        {
            return new PaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CardNumber = "5105105105105100",
                    ExpiryDate = new ExpiryDate
                    {
                        Month = 4,
                        Year = 2025
                    },
                    CVV = "123"
                },
                Amount = new Amount
                {
                    Currency = "GBP",
                    Value = 100
                }
            };
        }
    }
}
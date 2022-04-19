using Gateway.API.Models;

namespace Gateway.API.Services
{
    public interface IPaymentService
    {
        Guid InitiatePaymentFlow(PaymentRequest request);

        Task<PaymentDetails> RetrievePaymentDetailsAsync(Guid id);
    }
}

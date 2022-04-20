using Gateway.API.Models;

namespace Gateway.API.Services
{
    public interface IPaymentService
    {
        Guid InitiatePaymentFlow(PaymentRequest request, Guid merchantId);

        Task<PaymentDetails> RetrievePaymentDetailsAsync(Guid id);
    }
}

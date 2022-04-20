using Gateway.API.Models;

namespace Gateway.API.Services
{
    public interface IPaymentService
    {
        Task<Guid> InitiatePaymentFlow(PaymentRequest request, Guid merchantId);

        Task<PaymentDetails> RetrievePaymentDetails(Guid id);
    }
}

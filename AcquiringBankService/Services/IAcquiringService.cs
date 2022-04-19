using AcquiringBankService.Infrastructure.Events;
using AcquiringBankService.Models;

namespace AcquiringBankService.Services
{
    public interface IAcquiringService
    {
        Task<AcquiringBankResponse> Process(PaymentPersistedEvent request);
    }
}
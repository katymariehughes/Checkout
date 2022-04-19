using AutoMapper;

namespace AcquiringBankService.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Events.PaymentPersistedEvent, Models.AcquiringBankRequest>()
                .ConvertUsing(e => new Models.AcquiringBankRequest
                {
                    CardNumber = e.CardDetails.CardNumber,
                    ExpiryMonth = e.CardDetails.ExpiryDate.Month,
                    ExpiryYear = e.CardDetails.ExpiryDate.Year,
                    CVV = e.CardDetails.CVV,
                    Currency = e.Amount.Currency,
                    Amount = e.Amount.Value
                });

            CreateMap<Models.AcquiringBankResponse, Events.AcquirerResponseEvent>();
            CreateMap<Models.Source, Events.Source>();
        }
    }
}

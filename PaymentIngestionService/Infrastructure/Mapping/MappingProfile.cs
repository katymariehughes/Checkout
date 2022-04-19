using AutoMapper;

namespace PaymentIngestionService.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Events.PaymentRequestedEvent, Domain.Payment>();
            CreateMap<Events.CardDetails, Domain.CardDetails>();
            CreateMap<Events.ExpiryDate, Domain.ExpiryDate>();
            CreateMap<Events.Amount, Domain.Amount>();

            CreateMap<Domain.Payment, Events.PaymentPersistedEvent>()
                .ForMember(e => e.PaymentId, opt => opt.MapFrom(p => p.Id));

            CreateMap<Domain.CardDetails, Events.CardDetails>();
            CreateMap<Domain.ExpiryDate, Events.ExpiryDate>();
            CreateMap<Domain.Amount, Events.Amount>();
        }
    }
}

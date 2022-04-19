using AutoMapper;

namespace PaymentProcessingService.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Events.AcquirerResponseEvent, Domain.Authorization>()
                .ForMember(d => d.Type, opt => opt.MapFrom(e => e.Source.Type))
                .ForMember(d => d.Scheme, opt => opt.MapFrom(e => e.Source.Scheme))
                .ForMember(d => d.Last4, opt => opt.MapFrom(e => e.Source.Last4))
                .ForMember(d => d.Bin, opt => opt.MapFrom(e => e.Source.Bin))
                .ForMember(d => d.CardType, opt => opt.MapFrom(e => e.Source.CardType))
                .ForMember(d => d.Issuer, opt => opt.MapFrom(e => e.Source.Issuer))
                .ForMember(d => d.IssuerCountry, opt => opt.MapFrom(e => e.Source.IssuerCountry));
        }
    }
}

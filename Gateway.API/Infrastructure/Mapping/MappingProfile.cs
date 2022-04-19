using AutoMapper;
using Gateway.API.Infrastructure.Events;
using Gateway.API.Models;

namespace Gateway.API.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestedEvent>();
        }
    }
}

using AcquiringBankService.Infrastructure.Events;
using AcquiringBankService.Models;
using AutoMapper;
using Flurl.Http;
using Flurl.Http.Content;
using Microsoft.AspNetCore.DataProtection;
using System.Text;

namespace AcquiringBankService.Services
{
    public class AcquiringService : IAcquiringService
    {
        private readonly string _acquiringBankUrl;
        private readonly IDataProtector _dataProtector;
        private readonly IMapper _mapper;

        public AcquiringService(IConfiguration config, IDataProtectionProvider dataProtectionProvider, IMapper mapper)
        {
            _acquiringBankUrl = config.GetConnectionString("AcquiringBank");
            _dataProtector = dataProtectionProvider.CreateProtector("CardDetailsProtector");
            _mapper = mapper;
        }

        public async Task<AcquiringBankResponse> Process(PaymentPersistedEvent request)
        {
            var bankRequest = _mapper.Map<AcquiringBankRequest>(request);

            // bankRequest.CardNumber = _dataProtector.Unprotect(bankRequest.CardNumber);
            // bankRequest.CVV = _dataProtector.Unprotect(bankRequest.CVV);

            var json = FlurlHttp.GlobalSettings.JsonSerializer.Serialize(bankRequest);
            var content = new CapturedJsonContent(json);

            var result = await _acquiringBankUrl.PostAsync(content);
            return await result.GetJsonAsync<AcquiringBankResponse>();
            //var result = await _acquiringBankUrl.PostJsonAsync(request);

            //return await result.GetJsonAsync<AcquiringBankResponse>();
        }
    }
}

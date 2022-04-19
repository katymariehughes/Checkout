using AutoMapper;
using Common.Events;
using Dapper;
using DotNetCore.CAP;
using Gateway.API.Infrastructure.Dapper;
using Gateway.API.Infrastructure.Events;
using Gateway.API.Models;
using Microsoft.AspNetCore.DataProtection;
using System.Data;

namespace Gateway.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _publisher;
        private readonly DapperContext _context;

        public PaymentService(ILogger<PaymentService> logger, IDataProtectionProvider dataProtectionProvider, IMapper mapper, ICapPublisher publisher, DapperContext context)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("CardDetailsProtector");
            _logger = logger;
            _mapper = mapper;
            _publisher = publisher;
            _context = context;
        }

        public Guid InitiatePaymentFlow(PaymentRequest request)
        {
            _logger.LogInformation("Initiating payment flow");

            // request.CardDetails.CardNumber = _dataProtector.Protect(request.CardDetails.CardNumber);
            // request.CardDetails.CVV = _dataProtector.Protect(request.CardDetails.CVV);

            var paymentId = Guid.NewGuid();

            _publisher.Publish("payments.requested", _mapper.Map<PaymentRequestedEvent>(request), paymentId);

            return paymentId;
        }

        public async Task<PaymentDetails> RetrievePaymentDetailsAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<PaymentDetails>("sp_RetrievePaymentById", new { Id = id }, commandType: CommandType.StoredProcedure);
            return result.SingleOrDefault();
        }
    }
}

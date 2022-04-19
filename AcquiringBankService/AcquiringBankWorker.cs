using AcquiringBankService.Infrastructure.EntityFramework;
using AcquiringBankService.Infrastructure.Events;
using AcquiringBankService.Services;
using AutoMapper;
using Common.Events;
using DotNetCore.CAP;

namespace AcquiringBankService
{
    public sealed class AcquiringBankWorker : ICapSubscribe
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private const string ConsumingQueue = "payments.persisted";
        private const string ProducingQueue = "payments.acquired";

        public AcquiringBankWorker(ILogger<AcquiringBankWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        [CapSubscribe(ConsumingQueue)]
        public async Task HandleMessage(PaymentPersistedEvent requestedPayment, [FromCap] CapHeader header)
        {
            var (messageId, correlationId) = header.GetIds();

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AcquiringContext>();

            if (await context.HasBeenProcessed(messageId, nameof(AcquiringBankWorker)))
                return;

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var capPublisher = scope.ServiceProvider.GetRequiredService<ICapPublisher>();
            var acquiringBankService = scope.ServiceProvider.GetRequiredService<IAcquiringService>();

            var response = await acquiringBankService.Process(requestedPayment);

            using (var txn = context.Database.BeginTransaction(capPublisher))
            {
                await context.PersistIdemptencyToken(messageId, nameof(AcquiringBankWorker));

                capPublisher.Publish(ProducingQueue, mapper.Map<AcquirerResponseEvent>(response), correlationId);

                await txn.CommitAsync();
            }
        }
    }
}
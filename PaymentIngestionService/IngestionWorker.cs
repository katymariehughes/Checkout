using AutoMapper;
using Common.Events;
using DotNetCore.CAP;
using PaymentIngestionService.Domain;
using PaymentIngestionService.Infrastructure.EntityFramework;
using PaymentIngestionService.Infrastructure.Events;

namespace PaymentIngestionService
{
    public sealed class IngestionWorker : ICapSubscribe
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private const string ConsumingQueue = "payments.requested";
        private const string ProducingQueue = "payments.persisted";

        public IngestionWorker(ILogger<IngestionWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        [CapSubscribe(ConsumingQueue)]
        public async Task HandleMessage(PaymentRequestedEvent requestedPayment, [FromCap] CapHeader header)
        {
            var (messageId, correlationId) = header.GetIds();

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IngestionContext>();

            if (await context.HasBeenProcessed(messageId, nameof(IngestionWorker)))
                return;

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var capPublisher = scope.ServiceProvider.GetRequiredService<ICapPublisher>();

            using (var txn = context.Database.BeginTransaction(capPublisher))
            {
                var entity = mapper.Map<Payment>(requestedPayment);
                entity.Id = correlationId;

                context.Add(entity);
                await context.SaveChangesAsync();
                await context.PersistIdemptencyToken(messageId, nameof(IngestionWorker));

                capPublisher.Publish(ProducingQueue, mapper.Map<PaymentPersistedEvent>(entity), correlationId);

                await txn.CommitAsync();
            }
        }
    }
}
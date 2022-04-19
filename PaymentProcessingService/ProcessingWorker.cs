using AutoMapper;
using Common.Events;
using DotNetCore.CAP;
using PaymentProcessingService.Domain;
using PaymentProcessingService.Infrastructure.EntityFramework;
using PaymentProcessingService.Infrastructure.Events;

namespace PaymentProcessingService
{
    public sealed class ProcessingWorker : ICapSubscribe
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private const string ConsumingQueue = "payments.acquired";
        private const string ProducingQueue = "payments.processed";

        public ProcessingWorker(ILogger<ProcessingWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        [CapSubscribe(ConsumingQueue)]
        public async Task HandleMessage(AcquirerResponseEvent acquirerResponse, [FromCap] CapHeader header)
        {
            var (messageId, correlationId) = header.GetIds();

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProcessorContext>();

            if (await context.HasBeenProcessed(messageId, nameof(ProcessingWorker)))
                return;

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var capPublisher = scope.ServiceProvider.GetRequiredService<ICapPublisher>();

            using (var txn = context.Database.BeginTransaction(capPublisher))
            {
                var entity = mapper.Map<Authorization>(acquirerResponse);
                entity.Id = Guid.NewGuid();
                entity.PaymentId = correlationId;

                context.Add(entity);
                await context.SaveChangesAsync();
                await context.PersistIdemptencyToken(messageId, nameof(ProcessingWorker));

                capPublisher.Publish(ProducingQueue, new PaymentProcessedEvent { IsApproved = acquirerResponse.Approved }, correlationId);

                await txn.CommitAsync();
            }
        }
    }
}
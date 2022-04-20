using AcquiringBankService;
using Common.Events;
using DotNetCore.CAP;
using Moq;
using PaymentIngestionService;
using PaymentProcessingService;
using PaymentProcessingService.Infrastructure.EntityFramework;
using PaymentProcessingService.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class PaymentProcessingServiceTests : WorkerTestBase
    {
        [Fact]
        public async Task DoesNothingIfMessageHasAlreadyBeenProcessedAsync()
        {
            var context = new Mock<IProcessorContext>();
            context.Setup(x => x.HasBeenProcessed(It.IsAny<Guid>(), It.Is<string>(c => c == nameof(ProcessingWorker))))
                .ReturnsAsync(true);

            var serviceScopeFactory = RegisterDbContext(context.Object);
            var worker = new ProcessingWorker(serviceScopeFactory);

            await worker.HandleMessage(new(), new CapHeader(new Dictionary<string, string>
            {
                {"message-id", "43161b9f-4624-43d6-be4f-ea7b7c4c2a0b" },
                {"correlation-id", "43161b9f-4624-43d6-be4f-ea7b7c4c2a0b" }
            }));

            context.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Fact(Skip = "Need to mock BeginTransaction")]
        public async Task SavesToDbAndPublishesMessage()
        {
            var context = new Mock<IProcessorContext>();
            context.Setup(x => x.HasBeenProcessed(It.IsAny<Guid>(), It.Is<string>(c => c == nameof(ProcessingWorker))))
                .ReturnsAsync(false);

            var publisher = new Mock<ICapPublisher>();
            var serviceScopeFactory = RegisterDbContextAndPublisher(context.Object, publisher.Object);
            var worker = new IngestionWorker(serviceScopeFactory);

            var correlationId = Guid.Parse("43161b9f-4624-43d6-be4f-ea7b7c4c2a0b");
            await worker.HandleMessage(new(), new CapHeader(new Dictionary<string, string>
            {
                {"message-id", "43161b9f-4624-43d6-be4f-ea7b7c4c2a0b" },
                {"correlation-id", correlationId.ToString() }
            }));

            context.Verify(x => x.SaveChangesAsync(default), Times.Once);
            publisher.Verify(x => x.PublishAsync("payments.processed", It.IsAny<PaymentProcessedEvent>(), It.Is<Guid>(c => c == correlationId)), Times.Once);
        }
    }
}
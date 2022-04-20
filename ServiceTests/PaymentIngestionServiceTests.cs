using Common.Events;
using DotNetCore.CAP;
using Moq;
using PaymentIngestionService;
using PaymentIngestionService.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class PaymentIngestionServiceTests : WorkerTestBase
    {
        [Fact]
        public async Task DoesNothingIfMessageHasAlreadyBeenProcessedAsync()
        {
            var context = new Mock<IIngestionContext>();
            context.Setup(x => x.HasBeenProcessed(It.IsAny<Guid>(), It.Is<string>(c => c == nameof(IngestionWorker))))
                .ReturnsAsync(true);

            var serviceScopeFactory = RegisterDbContext(context.Object);
            var worker = new IngestionWorker(serviceScopeFactory);

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
            var context = new Mock<IIngestionContext>();
            context.Setup(x => x.HasBeenProcessed(It.IsAny<Guid>(), It.Is<string>(c => c == nameof(IngestionWorker))))
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
            publisher.Verify(x => x.PublishAsync("payments.persisted", It.IsAny<PaymentIngestionService.Infrastructure.Events.PaymentPersistedEvent>(), It.Is<Guid>(c => c == correlationId)), Times.Once);
        }
    }
}
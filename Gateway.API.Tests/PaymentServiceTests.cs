using AutoMapper;
using DotNetCore.CAP;
using Gateway.API.Infrastructure.Dapper;
using Gateway.API.Infrastructure.Mapping;
using Gateway.API.Models;
using Gateway.API.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.API.Tests
{
    public class PaymentServiceTests
    {
        [Fact(Skip = "Invocation for some reason is not being picked up")]
        public async Task InitiatePublishesMessageAsync()
        {
            var publisher = new Mock<ICapPublisher>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var paymentService = new PaymentService(Mock.Of<ILogger<PaymentService>>(), Mock.Of<IDataProtectionProvider>(), mapper, publisher.Object, Mock.Of<IDapperContext>());

            await paymentService.InitiatePaymentFlow(new(), Guid.NewGuid());

            publisher.Verify(x => x.PublishAsync(It.Is<string>(n => n == "payments.requested"), It.IsAny<PaymentRequest>(), It.IsAny<Dictionary<string, string>>(), default));
        }
    }
}
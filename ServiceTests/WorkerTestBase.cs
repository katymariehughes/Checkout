using AutoMapper;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentIngestionService.Infrastructure.Mapping;
using System;

namespace ServiceTests
{
    public abstract class WorkerTestBase
    {
        public IServiceScopeFactory RegisterDbContext<TContext>(TContext context) where TContext : class
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(TContext)))
                .Returns(context);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            serviceProvider
              .Setup(x => x.GetService(typeof(IMapper)))
              .Returns(mapper);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            return serviceScopeFactory.Object;
        }

        public IServiceScopeFactory RegisterDbContextAndPublisher<TContext>(TContext context, ICapPublisher publisher) where TContext : class
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(TContext)))
                .Returns(context);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            serviceProvider
              .Setup(x => x.GetService(typeof(IMapper)))
              .Returns(mapper);

            serviceProvider
              .Setup(x => x.GetService(typeof(ICapPublisher)))
              .Returns(publisher);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            return serviceScopeFactory.Object;
        }
    }
}
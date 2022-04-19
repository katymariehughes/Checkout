using Common;
using Common.Events;
using Microsoft.EntityFrameworkCore;
using PaymentProcessingService;
using PaymentProcessingService.Infrastructure.EntityFramework;
using PaymentProcessingService.Infrastructure.Mapping;
using System.Reflection;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureServices((context, services) =>
{
    var config = context.Configuration;

    services.AddTransient<CapFilter>();
    services.AddTransient<ProcessingWorker>();
    services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
    services.AddDbContext<ProcessorContext>(opt => opt.UseSqlServer(config.GetConnectionString("Database")));
    services.AddCap(x =>
    {
        x.DefaultGroupName = "paymentprocessingservice";
        x.UseEntityFramework<ProcessorContext>();
        x.UseSqlServer(config.GetConnectionString("Database"));
        x.UseRabbitMQ(config.GetValue<string>("RabbitMQ:Host"));
    }).AddSubscribeFilter<CapFilter>();
});

hostBuilder.AddLogging("Seq");

IHost host = hostBuilder.Build();

await host.RunAsync();
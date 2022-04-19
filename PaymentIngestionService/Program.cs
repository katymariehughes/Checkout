using Common;
using Common.Events;
using Microsoft.EntityFrameworkCore;
using PaymentIngestionService;
using PaymentIngestionService.Infrastructure.EntityFramework;
using PaymentIngestionService.Infrastructure.Mapping;
using System.Reflection;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureServices((context, services) =>
{
    var config = context.Configuration;

    //services.AddHostedService<IngestionWorker>();
    services.AddTransient<CapFilter>();
    services.AddTransient<IngestionWorker>();
    services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
    services.AddDbContext<IngestionContext>(opt => opt.UseSqlServer(config.GetConnectionString("Database")));
    services.AddCap(x =>
    {
        x.DefaultGroupName = "paymentingestionservice";
        x.UseEntityFramework<IngestionContext>(opt => opt.Schema = "ing");
        x.UseSqlServer(config.GetConnectionString("Database"));
        x.UseRabbitMQ(config.GetValue<string>("RabbitMQ:Host"));
    }).AddSubscribeFilter<CapFilter>();
});

hostBuilder.AddLogging("Seq", "PaymentIngestionService");

IHost host = hostBuilder.Build();

await host.RunAsync();

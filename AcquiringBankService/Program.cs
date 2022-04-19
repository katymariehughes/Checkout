using AcquiringBankService;
using AcquiringBankService.Infrastructure.EntityFramework;
using AcquiringBankService.Infrastructure.Http;
using AcquiringBankService.Infrastructure.Mapping;
using AcquiringBankService.Services;
using Common;
using Common.Events;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureServices((context, services) =>
{
    var config = context.Configuration;

    services.AddTransient<AcquiringBankWorker>();
    services.AddScoped<CapFilter>();
    services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
    services.AddDataProtection();
    services.AddDbContext<AcquiringContext>(opt => opt.UseSqlServer(config.GetConnectionString("Database")));
    services.AddScoped<IAcquiringService, AcquiringService>();
    FlurlHttp.Configure(settings => settings.HttpClientFactory = new PollyHttpClientFactory());

    services.AddCap(x =>
    {
        x.DefaultGroupName = "acquiringbankservice";
        x.UseEntityFramework<AcquiringContext>();
        x.UseSqlServer(config.GetConnectionString("Database"));
        x.UseRabbitMQ(config.GetValue<string>("RabbitMQ:Host"));
    }).AddSubscribeFilter<CapFilter>();
});

hostBuilder.AddLogging("Seq");

await hostBuilder.Build().RunAsync();

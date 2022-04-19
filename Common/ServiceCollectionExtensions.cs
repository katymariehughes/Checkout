using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Common
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLogging(this IHostBuilder hostBuilder, string seqConnectionStringName)
        {
            hostBuilder.UseSerilog((ctx, config) =>
            {
                config.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                      .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                      .MinimumLevel.Override("System", LogEventLevel.Warning)
                      .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                      .Enrich.WithMachineName()
                      .Enrich.WithSpan()
                      .Enrich.FromLogContext()
                      .WriteTo.Seq(ctx.Configuration.GetConnectionString(seqConnectionStringName))
                      .WriteTo.Console();
            });
        }

        public static void RegisterRabbitMqProducerConnection(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IConnection>(new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            }.CreateConnection());
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Common
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLogging(this IHostBuilder hostBuilder, string seqConnectionStringName, string applicationName)
        {
            hostBuilder.UseSerilog((ctx, config) =>
            {
                config.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                      .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                      .MinimumLevel.Override("System", LogEventLevel.Warning)
                      .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                      .Enrich.WithMachineName()
                      .Enrich.WithSpan()
                      .Enrich.WithProperty("Application", applicationName)
                      .Enrich.FromLogContext()
                      .WriteTo.Seq(ctx.Configuration.GetConnectionString(seqConnectionStringName))
                      .WriteTo.Console();
            });
        }
    }
}
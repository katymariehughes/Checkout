using Common;
using Common.Events;
using FluentValidation.AspNetCore;
using Gateway.API.Infrastructure.Extensions;
using Gateway.API.Infrastructure.Mapping;
using Gateway.API.Infrastructure.Middleware;
using Gateway.API.Infrastructure.Validation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

builder.Services.AddControllers()
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<PaymentRequestValidator>(lifetime: ServiceLifetime.Singleton));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"c:\temp-keys\"));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.RegisterTypes(config);

builder.Services.AddCap(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("Database"));
    opt.UseRabbitMQ(config.GetValue<string>("RabbitMQ:Host"));
}).AddSubscribeFilter<CapFilter>();

builder.Host.AddLogging("Seq");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway.API v1"));

app.UseSerilogRequestLogging();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

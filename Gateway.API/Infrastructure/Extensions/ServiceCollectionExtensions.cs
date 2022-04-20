using Common;
using Common.Events;
using Gateway.API.Infrastructure.Dapper;
using Gateway.API.Infrastructure.Middleware;
using Gateway.API.Infrastructure.Swagger;
using Gateway.API.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Gateway.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterTypes(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IDapperContext, DapperContext>();
            services.AddSingleton<IDateTimeOracle, DateTimeOracle>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<CapFilter>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway.API", Version = "v1" });

                c.ExampleFilters();

                c.AddSecurityDefinition(ApiKeyMiddleware.HeaderName, new OpenApiSecurityScheme
                {
                    Description = "Test API key - X-API-KEY: pgH7QzFHJx4w46fI5Uzi4RvtTwlEXp",
                    In = ParameterLocation.Header,
                    Name = ApiKeyMiddleware.HeaderName,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = ApiKeyMiddleware.HeaderName,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiKeyMiddleware.HeaderName
                            },
                         },
                         new string[] {}
                     }
                });
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetAssembly(typeof(PaymentRequestExample)));
        }
    }
}
using FluentAssertions;
using Gateway.API.Infrastructure.Exceptions;
using Gateway.API.Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.API.Tests
{
    public class ApiKeyMiddlewareTests
    {
        [Fact]
        public void ThrowsUnauthorizedIfNoHeaderPresent()
        {
            var middleware = new ApiKeyMiddleware(Mock.Of<RequestDelegate>(), new Mock<IConfiguration>().Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("SOME-OTHER-HEADER","blabla");

            Func<Task> act = async () => await middleware.InvokeAsync(httpContext);

            act.Should().ThrowAsync<UnauthorizedException>().WithMessage("Request was missing an API key.");
        }

        [Fact]
        public void ThrowsUnauthorizedIfNoMatchingMerchantId()
        {
            var middleware = new ApiKeyMiddleware(Mock.Of<RequestDelegate>(), new Mock<IConfiguration>().Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("X-API-KEY", "blabla");

            Func<Task> act = async () => await middleware.InvokeAsync(httpContext);

            act.Should().ThrowAsync<UnauthorizedException>().WithMessage("Unauthorized client.");
        }

        [Fact]
        public async Task AddsMerchantIdToContextItemsAsync()
        {
            var apiKey = "blabla";
            var merchantId = Guid.NewGuid();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {$"MerchantApiKeys:{apiKey}", merchantId.ToString()}
                })
                .Build();

            var middleware = new ApiKeyMiddleware(Mock.Of<RequestDelegate>(), configuration);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("X-API-KEY", apiKey);

            await middleware.InvokeAsync(httpContext);

            httpContext.Items.Should().Contain(new KeyValuePair<object, object>("MerchantId", merchantId));
        }
    }
}
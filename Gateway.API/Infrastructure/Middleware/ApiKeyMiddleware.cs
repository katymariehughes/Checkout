using Gateway.API.Infrastructure.Exceptions;

namespace Gateway.API.Infrastructure.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public const string HeaderName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedApiKey))
                throw new UnauthorizedException("Request was missing an API key.");

            Guid merchantId = _configuration.GetValue<Guid>($"MerchantApiKeys:{extractedApiKey}");

            if (merchantId == Guid.Empty)
                throw new UnauthorizedException("Unauthorized client.");

            context.Request.HttpContext.Items.Add("MerchantId", merchantId);

            await _next(context);
        }
    }
}

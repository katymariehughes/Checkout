using Gateway.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.API.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            object errors = null;
            switch (ex)
            {
                case RestException re:
                    _logger.LogError(ex, "REST Error");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    _logger.LogError(ex, "Server Error");
                    errors = "Error";
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                var problemDetails = JsonConvert.SerializeObject(new ProblemDetails
                {
                    Title = "An error occurred",
                    Status = context.Response.StatusCode,
                    Detail = errors is null ? null : JsonConvert.SerializeObject(errors),
                    Instance = Guid.NewGuid().ToString()
                });

                await context.Response.WriteAsync(problemDetails);
            }
        }
    }
}

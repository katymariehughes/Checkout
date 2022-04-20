using System.Net;

namespace Gateway.API.Infrastructure.Exceptions
{
    public class RequestValidationException : RestException
    {
        public RequestValidationException(IEnumerable<string> errors, string message = null) 
            : base(HttpStatusCode.BadRequest, new { Message = message ?? "Validation error occurred.", Errors = errors }) { }
    }
}

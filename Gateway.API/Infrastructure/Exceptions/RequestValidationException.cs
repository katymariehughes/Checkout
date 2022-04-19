using System.Net;

namespace Gateway.API.Infrastructure.Exceptions
{
    public class RequestValidationException : RestException
    {
        public RequestValidationException(IEnumerable<string> errors) 
            : base(HttpStatusCode.BadRequest, new { Message = "Validation error occurred.", Errors = errors }) { }
    }
}

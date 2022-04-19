using System.Net;

namespace Gateway.API.Infrastructure.Exceptions
{
    public class UnauthorizedException : RestException
    {
        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, new { Message = message }) { }
    }
}

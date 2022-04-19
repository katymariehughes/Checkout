using System.Net;

namespace Gateway.API.Infrastructure.Exceptions
{
    public class NotFoundException : RestException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, new { Message = message }) { }
    }
}

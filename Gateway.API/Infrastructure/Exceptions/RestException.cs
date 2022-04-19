using System.Net;

namespace Gateway.API.Infrastructure.Exceptions
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; }

        public RestException(HttpStatusCode code, object errors = null)
        {
            Errors = errors;
            Code = code;
        }
    }
}

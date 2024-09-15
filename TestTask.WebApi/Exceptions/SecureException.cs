using System.Net;

namespace TestTask.WebApi.Exceptions
{
    public class SecureException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public SecureException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

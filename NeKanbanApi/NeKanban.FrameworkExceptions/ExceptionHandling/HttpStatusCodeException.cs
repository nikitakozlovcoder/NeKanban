using System.Net;

namespace NeKanban.Api.FrameworkExceptions.ExceptionHandling;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCode Status { get; }

    public HttpStatusCodeException(HttpStatusCode status, string msg = "") : base(msg)
    {
        Status = status;
    }
}
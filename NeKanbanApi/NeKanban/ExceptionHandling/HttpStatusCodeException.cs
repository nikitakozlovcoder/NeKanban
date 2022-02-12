using System.Net;

namespace NeKanban.ExceptionHandling;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCode Status { get; private set; }

    public HttpStatusCodeException(HttpStatusCode status, string msg = "") : base(msg)
    {
        Status = status;
    }
}
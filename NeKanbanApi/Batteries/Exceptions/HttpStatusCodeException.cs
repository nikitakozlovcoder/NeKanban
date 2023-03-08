using System.Net;

namespace Batteries.Exceptions;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCode Status { get; }

    public HttpStatusCodeException(HttpStatusCode status, string msg = "") : base(msg)
    {
        Status = status;
    }
}
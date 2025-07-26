using System.Net;
using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class NetworkError : Error
{
    public NetworkError(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
        WithMetadata("StatusCode", statusCode);
    }

    public NetworkError(string message, Exception exception)
        : base(message)
    {
        CausedBy(exception);
    }

    public HttpStatusCode StatusCode { get; }
}

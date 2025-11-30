using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class GameApiClientError : Error
{
    public GameApiClientError(string message, Exception exception)
        : base(message)
    {
        CausedBy(exception);
    }
}

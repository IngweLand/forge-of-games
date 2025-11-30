using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class InGameDataDecodingError : Error
{
    protected InGameDataDecodingError()
    {
    }

    public InGameDataDecodingError(Exception innerException)
    {
        CausedBy(innerException);
    }
}

using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class HohMappingError : Error
{
    public HohMappingError(string message) : base(message)
    {
    }

    public HohMappingError(string message, Exception exception) : base(message)
    {
        CausedBy(exception);
    }
}

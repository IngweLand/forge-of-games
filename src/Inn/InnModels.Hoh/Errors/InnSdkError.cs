using FluentResults;

namespace Ingweland.Fog.Inn.Models.Hoh.Errors;

public abstract class InnSdkError : Error
{
    protected InnSdkError(string message) : base(message)
    {
    }

    protected InnSdkError(string message, Exception exception) : base(message)
    {
        CausedBy(exception);
    }
}

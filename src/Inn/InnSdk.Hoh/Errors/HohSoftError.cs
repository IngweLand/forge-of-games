using FluentResults;
using Ingweland.Fog.Inn.Models.Hoh;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class HohSoftError : Error
{
    public HohSoftError(SoftErrorType error) : base($"HoH soft error: {error}.")
    {
        Error = error;
        WithMetadata("Error", error);
    }

    public SoftErrorType Error { get; }
}

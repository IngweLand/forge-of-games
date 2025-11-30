using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class RankingsUpsertionError : Error
{
    public RankingsUpsertionError(Exception? innerException = null) : base("Error upserting rankings.")
    {
        CausedBy(innerException);
    }
}

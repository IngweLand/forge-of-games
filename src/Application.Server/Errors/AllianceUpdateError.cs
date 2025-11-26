using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Errors;

public class AllianceUpdateError : Error
{
    public AllianceUpdateError(AllianceKey key, Exception? innerException = null) :
        base($"Error updating alliance. Alliance key {key.WorldId}:{key.InGameAllianceId}.")
    {
        InGameAllianceId = key.InGameAllianceId;
        WorldId = key.WorldId;
        CausedBy(innerException);
        WithMetadata("WorldId", key.WorldId);
        WithMetadata("InGameAllianceId", key.InGameAllianceId);
    }

    public int InGameAllianceId { get; }
    public string WorldId { get; }
}

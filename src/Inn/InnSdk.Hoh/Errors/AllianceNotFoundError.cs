using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class AllianceNotFoundError : Error
{
    public int AllianceId { get; }
    public string WorldId { get; }

    public AllianceNotFoundError(int allianceId, string worldId) :
        base($"Alliance with ID {allianceId} not found in world {worldId}.")
    {
        AllianceId = allianceId;
        WorldId = worldId;
        WithMetadata("AllianceId", allianceId);
        WithMetadata("WorldId", worldId);
    }
}

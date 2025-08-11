using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Errors;

public class OldAllianceDataError : Error
{
    public OldAllianceDataError(AllianceKey key) :
        base($"Provided alliance with key {key.WorldId}:{key.InGameAllianceId} is older than existing.")
    {
        InGameAllianceId = key.InGameAllianceId;
        WorldId = key.WorldId;
        WithMetadata("WorldId", key.WorldId);
        WithMetadata("InGameAllianceId", key.InGameAllianceId);
    }

    public int InGameAllianceId { get; }
    public string WorldId { get; }
}

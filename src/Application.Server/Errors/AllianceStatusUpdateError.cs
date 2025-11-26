using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class AllianceStatusUpdateError : Error
{
    public AllianceStatusUpdateError(int allianceId, Exception? innerException = null) :
        base($"Error updating alliance status. Alliance id {allianceId}.")
    {
        AllianceId = allianceId;
        CausedBy(innerException);
        WithMetadata("AllianceId", allianceId);
    }

    public int AllianceId { get; }
}

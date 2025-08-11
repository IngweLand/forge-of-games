using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class PlayerUpsertionError : Error
{
    public PlayerUpsertionError(string worldId, int inGamePlayerId, Exception? innerException = null) :
        base($"Error upserting player with key {worldId}:{inGamePlayerId}.")
    {
        InGamePlayerId = inGamePlayerId;
        WorldId = worldId;
        CausedBy(innerException);
        WithMetadata("WorldId", worldId);
        WithMetadata("InGamePlayerId", inGamePlayerId);
    }

    public int InGamePlayerId { get; }
    public string WorldId { get; }
}

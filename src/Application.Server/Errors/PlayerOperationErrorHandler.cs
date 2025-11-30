using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public abstract class PlayerOperationError : Error
{
    protected PlayerOperationError(
        string operation,
        string worldId,
        int inGamePlayerId,
        Exception? innerException = null)
        : base($"Error {operation} player with key {worldId}:{inGamePlayerId}.")
    {
        WorldId = worldId;
        InGamePlayerId = inGamePlayerId;
        CausedBy(innerException);
        WithMetadata("WorldId", worldId);
        WithMetadata("InGamePlayerId", inGamePlayerId);
    }

    public int InGamePlayerId { get; }

    public string WorldId { get; }
}

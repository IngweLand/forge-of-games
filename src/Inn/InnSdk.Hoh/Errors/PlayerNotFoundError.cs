using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class PlayerNotFoundError : Error
{
    public int PlayerId { get; }
    public string WorldId { get; }

    public PlayerNotFoundError(int playerId, string worldId) :
        base($"Player with ID {playerId} not found in world {worldId}.")
    {
        PlayerId = playerId;
        WorldId = worldId;
        WithMetadata("PlayerId", playerId);
        WithMetadata("WorldId", worldId);
    }
}

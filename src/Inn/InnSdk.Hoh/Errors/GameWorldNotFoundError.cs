using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class GameWorldNotFoundError : Error
{
    public GameWorldNotFoundError(string worldId) : base($"Game world with ID {worldId} not found.")
    {
        WorldId = worldId;
        WithMetadata("WorldId", worldId);
    }

    public string WorldId { get; }
}

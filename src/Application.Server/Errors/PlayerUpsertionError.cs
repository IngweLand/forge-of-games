namespace Ingweland.Fog.Application.Server.Errors;

public class PlayerUpsertionError : PlayerOperationError
{
    public PlayerUpsertionError(string worldId, int inGamePlayerId, Exception? innerException = null)
        : base("upserting", worldId, inGamePlayerId, innerException)
    {
    }
}

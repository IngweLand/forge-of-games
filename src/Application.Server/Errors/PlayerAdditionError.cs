namespace Ingweland.Fog.Application.Server.Errors;

public class PlayerAdditionError : PlayerOperationError
{
    public PlayerAdditionError(string worldId, int inGamePlayerId, Exception? innerException = null)
        : base("adding", worldId, inGamePlayerId, innerException)
    {
    }
}

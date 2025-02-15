using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Providers;

public class GameWorldsProvider : IGameWorldsProvider
{
    public IReadOnlyCollection<GameWorldConfig> GetGameWorlds()
    {
        return new List<GameWorldConfig>()
        {
            new("un", 1, "www"),
            new("zz", 1, "beta"),
        };
    }
}

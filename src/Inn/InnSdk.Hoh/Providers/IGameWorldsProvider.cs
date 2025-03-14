using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Providers;

public interface IGameWorldsProvider
{
    IReadOnlyCollection<GameWorldConfig> GetGameWorlds();
}

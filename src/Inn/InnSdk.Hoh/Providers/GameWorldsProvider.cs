using FluentResults;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Errors;

namespace Ingweland.Fog.InnSdk.Hoh.Providers;

public class GameWorldsProvider : IGameWorldsProvider
{
    public IReadOnlyCollection<GameWorldConfig> GetGameWorlds()
    {
        return new List<GameWorldConfig>
        {
            new("un", 1, "www"),
            new("zz", 1, "beta"),
        };
    }

    public Result<GameWorldConfig> Get(string id)
    {
        var gw = GetGameWorlds().FirstOrDefault(x => x.Id == id);
        return gw != null ? Result.Ok(gw) : Result.Fail(new GameWorldNotFoundError(id));
    }
}

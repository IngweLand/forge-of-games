using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Functions.Validators;

public class WorldValidator(IGameWorldsProvider gameWorldsProvider)
{
    public bool ValidateWorld(string responseUrl, out string errorMessage)
    {
        errorMessage = string.Empty;

        var worldId = UriUtils.GetSubdomain(responseUrl);
        if (gameWorldsProvider.GetGameWorlds().All(gw => gw.Id != worldId))
        {
            errorMessage = $"Invalid world id {worldId}.";
            return false;
        }

        return true;
    }
}
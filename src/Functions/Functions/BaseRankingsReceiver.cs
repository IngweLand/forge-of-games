using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public abstract class BaseRankingsReceiver(
    ILogger<BaseRankingsReceiver> logger,
    IGameWorldsProvider gameWorldsProvider)
{

    protected void SetDebugCorsHeaders(HttpRequest req)
    {
#if DEBUG
        // Set CORS headers
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST, OPTIONS");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
#endif
    }

    protected bool ValidateRequest(InGameDataRequestDto inGameData, string expectedPath, out string worldId)
    {
        worldId = string.Empty;
        
        var path = UriUtils.GetPath(inGameData.Url);
        if (path != expectedPath)
        {
            logger.LogError("Expected {ExpectedGameUrlPath} but got {path}", expectedPath, path);
            return false;
        }
        var tempWorldId = UriUtils.GetSubdomain(inGameData.Url);
        if (gameWorldsProvider.GetGameWorlds().All(gw => gw.Id != tempWorldId))
        {
            return false;
        }

        worldId = tempWorldId;
        return true;
    }
}
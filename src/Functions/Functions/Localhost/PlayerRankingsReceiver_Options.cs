using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions.Localhost;

public class PlayerRankingsReceiver_Options
{
    [Function("PlayerRankingsReceiver_Options")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "options", Route = "hoh/inGameData/{optional?}/{optional2?}/{optional3?}")]
        HttpRequest req)
    {
        // Set CORS headers
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST, OPTIONS");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");

        return new OkResult();
    }
}

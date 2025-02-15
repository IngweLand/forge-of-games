using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ingweland.Fog.Functions.Functions;

public class PlayerRankingsReceiver(
    ILogger<PlayerRankingsReceiver> logger,
    IInGameDataParsingService inGameDataParsingService,
    IPlayerRankingTableRepository playerRankingTableRepository,
    IGameWorldsProvider gameWorldsProvider):BaseRankingsReceiver(logger, gameWorldsProvider)
{
    [Function("PlayerRankingsReceiver")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hoh/inGameData/rankings/players")]
        HttpRequest req,
        [FromBody] InGameDataRequestDto inGameData)
    {
        SetDebugCorsHeaders(req);
        
        if (!ValidateRequest(inGameData, GameEndpoints.PlayerRankingPath, out var worldId))
        {
            return new BadRequestResult();
        }

        var ranks = inGameDataParsingService.ParsePlayerRanking(inGameData.Data);
        if (!Enum.TryParse(ranks.Type.ToString(), out Models.Hoh.Enums.PlayerRankingType playerRankingType))
        {
            logger.LogError("Cannot map {From} to {To}", typeof(PlayerRankingType).FullName,
                typeof(Models.Hoh.Enums.PlayerRankingType).FullName);

            return new BadRequestResult();
        }

        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        try
        {
            await playerRankingTableRepository.SaveAsync(ranks.SurroundingRanking, worldId, playerRankingType, date);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving player rankings.");
        }

        return new NoContentResult();
    }
}

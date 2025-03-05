using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Functions.Functions.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class AllianceRankingsReceiver(
    ILogger<AllianceRankingsReceiver> logger,
    IInGameDataParsingService inGameDataParsingService,
    IAllianceRankingTableRepository allianceRankingTableRepository,
    IAllianceRankingRawDataTableRepository allianceRankingRawDataTableRepository,
    IGameWorldsProvider gameWorldsProvider,
    DatabaseWarmUpService databaseWarmUpService) : RankingsReceiverBase(logger, gameWorldsProvider)
{
    [Function("AllianceRankingsReceiver")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hoh/inGameData/rankings/alliances")]
        HttpRequest req,
        [Microsoft.Azure.Functions.Worker.Http.FromBody]
        InGameDataRequestDto inGameData)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        
        SetDebugCorsHeaders(req);

        if (!ValidateRequest(inGameData, GameEndpoints.AllianceRankingPath, out var worldId))
        {
            return new BadRequestResult();
        }

        var ranks = inGameDataParsingService.ParseAllianceRankings(inGameData.Data);
        if (!Enum.TryParse(ranks.Type.ToString(), out AllianceRankingType allianceRankingType))
        {
            logger.LogError("Cannot map {From} to {To}", typeof(Inn.Models.Hoh.AllianceRankingType).FullName,
                typeof(AllianceRankingType).FullName);

            return new BadRequestResult();
        }

        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        try
        {
            await allianceRankingTableRepository.SaveAsync(ranks.SurroundingRanking, worldId, allianceRankingType,
                date);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving alliance rankings.");
        }

        try
        {
            var rawData = new AllianceRankingRawData
            {
                Data = inGameDataParsingService.Decode(inGameData.Data),
                CollectedAt = DateTime.UtcNow
            };
            await allianceRankingRawDataTableRepository.SaveAsync(rawData, worldId, allianceRankingType);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving alliance rankings raw data.");
        }

        return new NoContentResult();
    }
}

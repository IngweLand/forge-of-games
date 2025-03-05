using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class PlayerRankingsProcessor(
    IGameWorldsProvider gameWorldsProvider,
    IPlayerRankingService playerRankingService,
    IPlayerRankingTableRepository playerRankingTableRepository,
    ILogger<PlayerRankingsProcessor> logger,
    DatabaseWarmUpService databaseWarmUpService)
{
    [Function("PlayerRankingsProcessor")]
    public async Task Run([TimerTrigger("0 5 0 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        const PlayerRankingType playerRankingType = PlayerRankingType.RankingPoints;
        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            IReadOnlyCollection<PlayerRank> rankings;
            try
            {
                rankings = await playerRankingTableRepository.GetAllAsync(gameWorld.Id, playerRankingType, date);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not get player rankings for {@Summary}",
                    new {gameWorld.Id, playerRankingType, date});
                continue;
            }

            await playerRankingService.AddOrUpdateRangeAsync(rankings, gameWorld.Id, date, playerRankingType);
        }
    }
}
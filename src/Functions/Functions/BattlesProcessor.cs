using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PvpBattle = Ingweland.Fog.Models.Hoh.Entities.Battle.PvpBattle;

namespace Ingweland.Fog.Functions.Functions;

public class BattlesProcessor(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IPvpBattleService pvpBattleService,
    IBattleService battleService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IBattleStatsService battleStatsService,
    ILogger<BattlesProcessor> logger,
    DatabaseWarmUpService databaseWarmUpService) : FunctionBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    [Function("BattlesProcessor")]
    public async Task Run([TimerTrigger("0 10 0 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var allPvpBattles = new List<(string WorldId, PvpBattle PvpBattle)>();
        var allBattleResults =
            new List<(string WorldId, BattleSummary BattleSummary, DateOnly PerformedAt, Guid? SubmissionId)>();
        var allBattleStats = new List<(string worldId, BattleStats battleStats)>();
        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        logger.LogInformation("BattlesProcessor started for date {date}", date);
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            logger.LogInformation("Processing game world {gameWorldId}", gameWorld.Id);

            var pvpBattles = await GetPvpBattles(gameWorld.Id, date);
            allPvpBattles.AddRange(pvpBattles);
            logger.LogInformation("{count} pvp battles retrieved for game world {gameWorldId}",
                pvpBattles.Count, gameWorld.Id);

            var battleResults = await GetBattleResults(gameWorld.Id, date);
            allBattleResults.AddRange(battleResults);
            logger.LogInformation("{count} battle results retrieved for game world {gameWorldId}",
                battleResults.Count, gameWorld.Id);

            var battleStats = await GetBattleStats(gameWorld.Id, date);
            allBattleStats.AddRange(battleStats);
            logger.LogInformation("{count} battle stats retrieved for game world {gameWorldId}",
                battleStats.Count, gameWorld.Id);

            logger.LogInformation("Completed processing game world {gameWorldId}", gameWorld.Id);
        }

        logger.LogInformation("Total pvp battles: {count}, Total battles: {count}, Total battle stats {count}",
            allPvpBattles.Count, allBattleResults.Count, allBattleStats.Count);

        logger.LogInformation("Starting pvp battles service update");
        await ExecuteSafeAsync(() => pvpBattleService.AddAsync(allPvpBattles), "");
        logger.LogInformation("Completed pvp battles service update");

        logger.LogInformation("Starting battles service update");
        await ExecuteSafeAsync(() => battleService.AddAsync(allBattleResults), "");
        logger.LogInformation("Completed battles service update");

        logger.LogInformation("Starting battle stats update");
        await ExecuteSafeAsync(() => battleStatsService.AddAsync(allBattleStats), "");
        logger.LogInformation("Completed battles stats service update");
    }
}

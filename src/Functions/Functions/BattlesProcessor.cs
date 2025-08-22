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
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IBattleStatsService battleStatsService,
    ILogger<BattlesProcessor> logger,
    DatabaseWarmUpService databaseWarmUpService) : FunctionBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    [Function(nameof(BattlesProcessor))]
    public async Task Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(BattlesProcessor));
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var allPvpBattles = new List<(string WorldId, PvpBattle PvpBattle)>();
        var allBattleStats = new List<BattleStats>();
        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        logger.LogInformation("BattlesProcessor started for date {date}", date);
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            logger.LogInformation("Processing game world {gameWorldId}", gameWorld.Id);

            var pvpBattles = await GetPvpBattles(gameWorld.Id, date);
            allPvpBattles.AddRange(pvpBattles);
            logger.LogInformation("{count} pvp battles retrieved for game world {gameWorldId}",
                pvpBattles.Count, gameWorld.Id);

            var battleStats = await GetBattleStats(gameWorld.Id, date);
            allBattleStats.AddRange(battleStats.Select(t => t.battleStats));
            logger.LogInformation("{count} battle stats retrieved for game world {gameWorldId}",
                battleStats.Count, gameWorld.Id);

            logger.LogInformation("Completed processing game world {gameWorldId}", gameWorld.Id);
        }

        logger.LogInformation("Total pvp battles: {count}, Total battle stats {count}", allPvpBattles.Count,
            allBattleStats.Count);

        logger.LogInformation("Starting pvp battles service update");
        await ExecuteSafeAsync(() => pvpBattleService.AddAsync(allPvpBattles), "");
        logger.LogInformation("Completed pvp battles service update");

        logger.LogInformation("Starting battle stats update");
        await ExecuteSafeAsync(() => battleStatsService.AddAsync(allBattleStats), "");
        logger.LogInformation("Completed battles stats service update");
    }
}

using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class PvpBattlesBulkUpdater(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IPvpBattleService pvpBattleService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    ILogger<PvpBattlesBulkUpdater> logger,
    DatabaseWarmUpService databaseWarmUpService) : OrchestratorBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger), IPvpBattlesBulkUpdater
{
    public async Task<bool> RunAsync()
    {
        logger.LogInformation("Starting PvpBattlesBulkUpdater");
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var allPvpBattles = new List<(string WorldId, PvpBattle PvpBattle)>();
        var limit = 60;
        var initDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-limit);
        for (var i = 0; i < limit; i++)
        {
            var date = initDate.AddDays(i);
            logger.LogInformation("Processing date {date}", date);
            foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
            {
                var pvpBattles = await GetPvpBattles(gameWorld.Id, date);
                allPvpBattles.AddRange(pvpBattles);
                logger.LogInformation("{count} pvp battles retrieved for game world {gameWorldId}",
                    pvpBattles.Count, gameWorld.Id);
            }

            if (allPvpBattles.Count >= 1000)
            {
                await ExecuteSafeAsync(() => pvpBattleService.AddAsync(allPvpBattles), "PvpBattlesBulkUpdater");
                allPvpBattles.Clear();
            }
        }
        
        if (allPvpBattles.Count >= 0)
        {
            await ExecuteSafeAsync(() => pvpBattleService.AddAsync(allPvpBattles), "PvpBattlesBulkUpdater");
        }
        
        logger.LogInformation("Completed PvpBattlesBulkUpdater");
        return false;
    }
}

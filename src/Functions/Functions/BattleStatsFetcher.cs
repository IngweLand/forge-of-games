using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Shared.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class BattleStatsFetcher(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IInnSdkClient innSdkClient,
    IBattleStatsService battleStatsService,
    ILogger<BattleStatsFetcher> logger) : FunctionBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    [Function("BattleStatsFetcher")]
    public async Task Run([TimerTrigger("0 20,50 6-23/1 * * *")] TimerInfo myTimer)
    {
        logger.LogInformation("BattleStatsFetcher started");
        var allBattleStatsIds = new HashSet<byte[]>(StructuralByteArrayComparer.Instance);

        var limit = 2;
        var initDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-limit);
        for (var i = 0; i < limit + 1; i++)
        {
            var d = initDate.AddDays(i);
            foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
            {
                var existingBattleStats = await GetBattleStats(gameWorld.Id, d);
                var existingBattleStatsIds = existingBattleStats.Select(t => t.battleStats.BattleId).ToList();
                allBattleStatsIds.UnionWith(existingBattleStatsIds);
            }
        }

        logger.LogInformation("{count} unique existing battle stats ids retrieved", allBattleStatsIds.Count);

        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            var battleResults = await GetBattleResults(gameWorld.Id, date);
            var battleIds = battleResults.Select(t => t.BattleSummary.BattleId)
                .ToHashSet(StructuralByteArrayComparer.Instance);

            var pvpBattles = await GetPvpBattles(gameWorld.Id, date);
            var pvpBattleIds = pvpBattles.Select(t => t.PvpBattle.Id).ToHashSet(StructuralByteArrayComparer.Instance);

            var allBattleIds = battleIds.Union(pvpBattleIds).ToHashSet(StructuralByteArrayComparer.Instance);
            logger.LogInformation("{count} unique battle ids retrieved for game world {gameWorldId}",
                allBattleIds.Count, gameWorld.Id);

            var battlesWithoutStats =
                allBattleIds.Except(allBattleStatsIds, StructuralByteArrayComparer.Instance).ToList();
            logger.LogInformation("Found {count} battles without stats for game world {gameWorldId}",
                battlesWithoutStats.Count, gameWorld.Id);
            await FetchBattleStats(gameWorld, battlesWithoutStats);
        }

        logger.LogInformation("Completed battles stats fetch.");
    }

    private async Task FetchBattleStats(GameWorldConfig gameWorld, IEnumerable<byte[]> battleIds)
    {
        foreach (var battleId in battleIds)
        {
            var delayTask = Task.Delay(500);
            byte[] data;
            var battleIdString = Convert.ToBase64String(battleId);
            try
            {
                data = await innSdkClient.BattleService.GetBattleStatsRawDataAsync(gameWorld, battleId);
                logger.LogInformation("Fetch battle stats for world: {WorldId}, id: {BattleId}", gameWorld.Id,
                    battleIdString);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not fetch battle stats for world {WorldId}, id {BattleId}", gameWorld.Id,
                    battleIdString);
                continue;
            }

            var now = DateTime.UtcNow;
            var rawData = new InGameRawData
            {
                Base64Data = Convert.ToBase64String(data),
                CollectedAt = now,
            };

            try
            {
                await InGameRawDataTableRepository.SaveAsync(rawData,
                    InGameRawDataTablePartitionKeyProvider.BattleStats(gameWorld.Id, DateOnly.FromDateTime(now)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error saving battle stats raw data: world {WorldId}, id {BattleId}", gameWorld.Id,
                    battleIdString);
                continue;
            }
            
            BattleStats battleStats;
            try
            {
                battleStats = InGameDataParsingService.ParseBattleStats(rawData.Base64Data);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing battle stats raw data: world {WorldId}, id {BattleId}", gameWorld.Id,
                    battleIdString);
                continue;
            }

            await ExecuteSafeAsync(() => battleStatsService.AddAsync(battleStats),
                $"Error saving battle stats: world {gameWorld.Id}, id {battleIdString}");
            
            await delayTask;
        }
    }
}

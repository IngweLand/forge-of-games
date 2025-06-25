using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TempProcessor(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IBattleStatsService battleStatsService,
    ILogger<TempProcessor> logger,
    DatabaseWarmUpService databaseWarmUpService)
{
    [Function("TempProcessor")]
    public async Task Run([TimerTrigger("0 0 0 1 1 *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var allBattleStats = new List<(string worldId, BattleStats battleStats)>();
        var limit = 30;
        var initDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-limit);
        for (var i = 0; i < limit + 1; i++)
        {
            var date = initDate.AddDays(i);
            logger.LogInformation("TempProcessor started for date {date}", date);
            foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
            {
                var battleStats = await GetBattleStats(gameWorld.Id, date);
                allBattleStats.AddRange(battleStats);
                logger.LogInformation("{count} battle stats retrieved for game world {gameWorldId}",
                    battleStats.Count, gameWorld.Id);

                logger.LogInformation("Completed processing game world {gameWorldId}", gameWorld.Id);
            }
        }

        logger.LogInformation("Starting battle stats update");
        await ExecuteSafeAsync(() => battleStatsService.AddAsync(allBattleStats), "");
        logger.LogInformation("Completed battles stats service update");
    }

    private async Task ExecuteSafeAsync(Func<Task> func, string errorMessage)
    {
        try
        {
            await func();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while executing safe operation: {ErrorMessage}", errorMessage);
        }
    }

    private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> func, string errorMessage, T fallback)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while executing safe operation: {ErrorMessage}", errorMessage);
            return fallback;
        }
    }

    private async Task<List<(string worldId, BattleStats battleStats)>> GetBattleStats(string worldId,
        DateOnly date)
    {
        var rawDataItems = await ExecuteSafeAsync(
            () => inGameRawDataTableRepository.GetAllAsync(
                inGameRawDataTablePartitionKeyProvider.BattleStats(worldId, date)),
            $"Error getting battle stats raw data for world {worldId} on {date}", []);
        var result = new List<(string worldId, BattleStats battleStats)>();
        foreach (var rawData in rawDataItems)
        {
            try
            {
                result.Add((worldId, inGameDataParsingService.ParseBattleStats(rawData.Base64Data)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing battle stats raw data collected on {date}",
                    rawData.CollectedAt);
            }
        }

        return result;
    }
}

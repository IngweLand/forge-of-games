using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration.Abstractions;

public abstract class OrchestratorBase(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    ILogger logger)
{
    protected IGameWorldsProvider GameWorldsProvider { get; } = gameWorldsProvider;

    protected IInGameDataParsingService InGameDataParsingService { get; } = inGameDataParsingService;

    protected InGameRawDataTablePartitionKeyProvider InGameRawDataTablePartitionKeyProvider { get; } =
        inGameRawDataTablePartitionKeyProvider;

    protected IInGameRawDataTableRepository InGameRawDataTableRepository { get; } = inGameRawDataTableRepository;

    protected async Task<List<(string WorldId, BattleSummary BattleSummary)>> GetBattleResults(string worldId,
        DateOnly date)
    {
        var rawDataItems = await ExecuteSafeAsync(
            () => InGameRawDataTableRepository.GetAllAsync(
                InGameRawDataTablePartitionKeyProvider.BattleCompleteWave(worldId, date)),
            $"Error getting battle complete wave raw data for world {worldId} on {date}", []);
        var result = new List<(string WorldId, BattleSummary BattleSummary)>();
        foreach (var rawData in rawDataItems)
        {
            try
            {
                var parsed = InGameDataParsingService.ParseBattleWaveResult(rawData.Base64Data);

                if (parsed.ResultStatus == BattleResultStatus.Undefined)
                {
                    continue;
                }

                result.Add((worldId, parsed));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing battle complete wave raw data collected on {date}",
                    rawData.CollectedAt);
            }
        }

        return result;
    }

    protected async Task<List<(string worldId, BattleStats battleStats)>> GetBattleStats(string worldId,
        DateOnly date)
    {
        var rawDataItems = await ExecuteSafeAsync(
            () => InGameRawDataTableRepository.GetAllAsync(
                InGameRawDataTablePartitionKeyProvider.BattleStats(worldId, date)),
            $"Error getting battle stats raw data for world {worldId} on {date}", []);
        var result = new List<(string worldId, BattleStats battleStats)>();
        foreach (var rawData in rawDataItems)
        {
            try
            {
                result.Add((worldId, InGameDataParsingService.ParseBattleStats(rawData.Base64Data)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing battle stats raw data collected on {date}",
                    rawData.CollectedAt);
            }
        }

        return result;
    }
    
    protected async Task<List<(string WorldId, PvpBattle PvpBattle)>> GetPvpBattles(string worldId, DateOnly date)
    {
        var pvpBattlesRawData = await ExecuteSafeAsync(
            () => InGameRawDataTableRepository.GetAllAsync(
                InGameRawDataTablePartitionKeyProvider.PvpBattles(worldId, date)),
            $"Error getting pvp battles raw data for world {worldId} on {date}", []);
        var pvpBattles = new List<(string WorldId, PvpBattle Battle)>();
        foreach (var rawData in pvpBattlesRawData)
        {
            try
            {
                pvpBattles.AddRange(InGameDataParsingService.ParsePvpBattles(rawData.Base64Data)
                    .Select(src => (worldId, src)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing pvp battles raw data collected on {date}", rawData.CollectedAt);
            }
        }

        return pvpBattles;
    }

    protected async Task ExecuteSafeAsync(Func<Task> func, string errorMessage)
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

    protected async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> func, string errorMessage, T fallback)
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
}
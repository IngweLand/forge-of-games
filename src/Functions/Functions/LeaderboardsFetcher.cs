using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AllianceRankingType = Ingweland.Fog.Inn.Models.Hoh.AllianceRankingType;
using PlayerRankingType = Ingweland.Fog.Inn.Models.Hoh.PlayerRankingType;

namespace Ingweland.Fog.Functions.Functions;

public class LeaderboardsFetcher(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    ILogger<LeaderboardsFetcher> logger,
    DatabaseWarmUpService databaseWarmUpService)
{
    [Function("LeaderboardsFetcher")]
    public async Task Run([TimerTrigger("0 30 23 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            await FetchPlayerRankingsRawData(gameWorld);
            await FetchAllianceRankingsRawData(gameWorld);
        }
    }

    private async Task FetchAllianceRankingsRawData(GameWorldConfig gameWorld)
    {
        byte[] data;
        try
        {
            data = await innSdkClient.RankingsService.GetAllianceRankingRawDataAsync(gameWorld,
                AllianceRankingType.RankingPoints);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not fetch alliance rankings raw data for {WorldId}", gameWorld.Id);
            return;
        }

        try
        {
            var now = DateTime.UtcNow;
            var rawData = new InGameRawData
            {
                Base64Data = Convert.ToBase64String(data),
                CollectedAt = now
            };

            await inGameRawDataTableRepository.SaveAsync(rawData,
                inGameRawDataTablePartitionKeyProvider.AllianceRankings(gameWorld.Id, DateOnly.FromDateTime(now),
                    Models.Hoh.Enums.AllianceRankingType.RankingPoints));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving alliance rankings raw data.");
        }
    }

    private async Task FetchPlayerRankingsRawData(GameWorldConfig gameWorld)
    {
        byte[] data;
        try
        {
            data = await innSdkClient.RankingsService.GetPlayerRankingRawDataAsync(gameWorld,
                PlayerRankingType.RankingPoints);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not fetch player rankings raw data for {WorldId}", gameWorld.Id);
            return;
        }

        try
        {
            var now = DateTime.UtcNow;
            var rawData = new InGameRawData
            {
                Base64Data = Convert.ToBase64String(data),
                CollectedAt = now
            };

            await inGameRawDataTableRepository.SaveAsync(rawData,
                inGameRawDataTablePartitionKeyProvider.PlayerRankings(gameWorld.Id, DateOnly.FromDateTime(now),
                    Models.Hoh.Enums.PlayerRankingType.RankingPoints));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving player rankings raw data.");
        }
    }
}
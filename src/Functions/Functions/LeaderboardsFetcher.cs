using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AllianceRankingType = Ingweland.Fog.Inn.Models.Hoh.AllianceRankingType;
using PlayerRankingType = Ingweland.Fog.Inn.Models.Hoh.PlayerRankingType;

namespace Ingweland.Fog.Functions.Functions;

public class LeaderboardsFetcher(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    IPlayerRankingService playerRankingService,
    IAllianceRankingService allianceRankingService,
    IPlayerRankingTableRepository playerRankingTableRepository,
    IAllianceRankingTableRepository allianceRankingTableRepository,
    IAllianceRankingRawDataTableRepository allianceRankingRawDataTableRepository,
    ILogger<LeaderboardsFetcher> logger,
    DatabaseWarmUpService databaseWarmUpService)
{
    [Function("LeaderboardsFetcher")]
    public async Task Run([TimerTrigger("0 30 23 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        
        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            try
            {
                await FetchPlayerRankings(gameWorld);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error fetching player rankings");
            }

            try
            {
                await FetchAllianceRankingsRawData(gameWorld);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error fetching alliance rankings raw data");
            }
            
            try
            {
                await FetchAllianceRankings(gameWorld);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error fetching alliance rankings");
            }
        }
    }

    private async Task FetchPlayerRankings(GameWorldConfig gameWorld)
    {
        PlayerRanks ranks;
        try
        {
            ranks = await innSdkClient.RankingsService.GetPlayerRankingAsync(gameWorld,
                PlayerRankingType.RankingPoints);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not fetch player rankings for {WorldId}", gameWorld.Id);
            return;
        }

        if (!Enum.TryParse(ranks.Type.ToString(), out Models.Hoh.Enums.PlayerRankingType playerRankingType))
        {
            throw new ArgumentException($"Cannot map {typeof(PlayerRankingType).FullName} to {
                typeof(Models.Hoh.Enums.PlayerRankingType).FullName}");
        }

        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        await UpdateTableStorage(ranks.Top100, playerRankingType, gameWorld.Id, date);
        await UpdateTableStorage(ranks.SurroundingRanking, playerRankingType, gameWorld.Id, date);
        await playerRankingService.AddOrUpdateRangeAsync(ranks.Top100, gameWorld.Id, date, playerRankingType);
        await playerRankingService.AddOrUpdateRangeAsync(ranks.SurroundingRanking, gameWorld.Id, date, playerRankingType);
    }

    private async Task FetchAllianceRankings(GameWorldConfig gameWorld)
    {
        AllianceRanks ranks;
        try
        {
            ranks = await innSdkClient.RankingsService.GetAllianceRankingAsync(gameWorld,
                AllianceRankingType.RankingPoints);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not fetch alliance rankings for {WorldId}", gameWorld.Id);
            return;
        }

        if (!Enum.TryParse(ranks.Type.ToString(), out Models.Hoh.Enums.AllianceRankingType allianceRankingType))
        {
            throw new ArgumentException($"Cannot map {typeof(AllianceRankingType).FullName} to {
                typeof(Models.Hoh.Enums.AllianceRankingType).FullName}");
        }

        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        await UpdateTableStorage(ranks.Top100, allianceRankingType, gameWorld.Id, date);
        await allianceRankingService.AddOrUpdateRangeAsync(ranks.Top100, gameWorld.Id, date, allianceRankingType);
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

        var rawData = new AllianceRankingRawData
        {
            Data = data,
            CollectedAt = DateTime.UtcNow
        };
        await allianceRankingRawDataTableRepository.SaveAsync(rawData, gameWorld.Id,
            Models.Hoh.Enums.AllianceRankingType.RankingPoints);
    }

    private async Task UpdateTableStorage(IReadOnlyCollection<PlayerRank> rankings,
        Models.Hoh.Enums.PlayerRankingType playerRankingType, string worldId, DateOnly date)
    {
        await playerRankingTableRepository.SaveAsync(rankings, worldId, playerRankingType, date);
    }

    private async Task UpdateTableStorage(IReadOnlyCollection<AllianceRank> rankings,
        Models.Hoh.Enums.AllianceRankingType allianceRankingType, string worldId, DateOnly date)
    {
        await allianceRankingTableRepository.SaveAsync(rankings, worldId, allianceRankingType, date);
    }
}

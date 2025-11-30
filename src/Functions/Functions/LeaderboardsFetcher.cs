using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AllianceRankingType = Ingweland.Fog.Inn.Models.Hoh.AllianceRankingType;

namespace Ingweland.Fog.Functions.Functions;

public class LeaderboardsFetcher(
    IGameWorldsProvider gameWorldsProvider,
    IInnSdkClient innSdkClient,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    ILogger<LeaderboardsFetcher> logger,
    DatabaseWarmUpService databaseWarmUpService,
    IMapper mapper)
{
    private static readonly HashSet<PlayerRankingType> PlayerRankingTypes =
        [PlayerRankingType.ResearchPoints, PlayerRankingType.TotalHeroPower];

    private static readonly HashSet<AllianceRankingType> AllianceRankingTypes = [AllianceRankingType.MemberTotal];

    [Function("LeaderboardsFetcher")]
    public async Task Run([TimerTrigger("0 57 23 * * *")] TimerInfo myTimer)
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
        foreach (var allianceRankingType in AllianceRankingTypes)
        {
            byte[] data;
            try
            {
                data = await innSdkClient.RankingsService.GetAllianceRankingRawDataAsync(gameWorld,
                    allianceRankingType);
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "Could not fetch alliance rankings raw data for world: {WorldId}, type: {AllianceRankingType}",
                    gameWorld.Id, allianceRankingType);
                continue;
            }

            try
            {
                var now = DateTime.UtcNow;
                var rawData = new InGameRawData
                {
                    Base64Data = Convert.ToBase64String(data),
                    CollectedAt = now,
                };

                await inGameRawDataTableRepository.SaveAsync(rawData,
                    inGameRawDataTablePartitionKeyProvider.AllianceRankings(gameWorld.Id, DateOnly.FromDateTime(now),
                        mapper.Map<Models.Hoh.Enums.AllianceRankingType>(allianceRankingType)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error saving alliance rankings raw data.");
            }
        }
    }

    private async Task FetchPlayerRankingsRawData(GameWorldConfig gameWorld)
    {
        foreach (var playerRankingType in PlayerRankingTypes)
        {
            var dataResult = await innSdkClient.RankingsService.GetPlayerRankingRawDataAsync(gameWorld,
                playerRankingType);
            if (dataResult.IsFailed)
            {
                dataResult.Log<LeaderboardsFetcher>(LogLevel.Error);
                logger.LogError(null,
                    "Could not fetch player rankings raw data for world: {WorldId}, type: {PlayerRankingType}",
                    gameWorld.Id, playerRankingType);
                
                continue;
            }

            try
            {
                var now = DateTime.UtcNow;
                var rawData = new InGameRawData
                {
                    Base64Data = Convert.ToBase64String(dataResult.Value),
                    CollectedAt = now,
                };

                await inGameRawDataTableRepository.SaveAsync(rawData,
                    inGameRawDataTablePartitionKeyProvider.PlayerRankings(gameWorld.Id, DateOnly.FromDateTime(now),
                        mapper.Map<PlayerRankingType>(playerRankingType)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error saving player rankings raw data.");
            }
        }
    }
}

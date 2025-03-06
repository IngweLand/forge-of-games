using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class AutoDataProcessor(
    IGameWorldsProvider gameWorldsProvider,
    IPlayerRankingService playerRankingService,
    IPvpRankingService pvpRankingService,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IPlayerService playerService,
    IPlayerAgeHistoryService playerAgeHistoryService,
    IPlayerNameHistoryService playerNameHistoryService,
    IAllianceService allianceService,
    IPlayerAllianceNameHistoryService playerAllianceNameHistoryService,
    IAllianceRankingService allianceRankingService,
    IAllianceNameHistoryService allianceNameHistoryService,
    IAllianceMembersService allianceMembersService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IMapper mapper,
    ILogger<AutoDataProcessor> logger,
    DatabaseWarmUpService databaseWarmUpService)
{
    private const PlayerRankingType PlayerRankingType = Models.Hoh.Enums.PlayerRankingType.RankingPoints;
    private const AllianceRankingType AllianceRankingType = Models.Hoh.Enums.AllianceRankingType.RankingPoints;

    [Function("AutoDataProcessor")]
    public async Task Run([TimerTrigger("0 5 0 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        var playerAggregates = new List<PlayerAggregate>(32000);
        var allianceAggregates = new List<AllianceAggregate>(16000);
        var allConfirmedAllianceMembers = new List<(DateTime CollectedAt, AllianceKey AllianceKey, IEnumerable<int>)>();
        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            var playerRankings = await GetPlayerRanking(gameWorld.Id, date);
            var pvpRankings = await GetPvpRanking(gameWorld.Id, date);
            var allianceRankings = await GetAllianceRanking(gameWorld.Id, date);
            var allianceWakeups =
                await GetWakeupsAsync(inGameRawDataTablePartitionKeyProvider.Alliance(gameWorld.Id, date));
            var alliances = allianceWakeups
                .Where(t => t.Wakeup.Alliance != null)
                .Select(t => (t.CollectedAt, Alliance: t.Wakeup.Alliance!))
                .ToList();
            var alliancesMembers = allianceWakeups
                .Where(t => t.Wakeup.AllianceWithMembers != null)
                .SelectMany(t => t.Wakeup.AllianceWithMembers!.Members.Select(m =>
                    (t.CollectedAt, t.Wakeup.AllianceWithMembers.AllianceId, m.Player)))
                .ToList();
            var confirmedAllianceMembers = allianceWakeups
                .Where(t => t.Wakeup.AllianceWithMembers != null)
                .Select(t => (t.CollectedAt,
                    new AllianceKey(gameWorld.Id, t.Wakeup.AllianceWithMembers!.AllianceId),
                    t.Wakeup.AllianceWithMembers!.Members.Select(m => m.Player.Id)))
                .ToList();
            allConfirmedAllianceMembers.AddRange(confirmedAllianceMembers);
            // var athAllianceRankingWakeups =
            //     await GetWakeupsAsync(
            //         inGameRawDataTablePartitionKeyProvider.AthAllianceRankings(gameWorld.Id, date));
            // var athAllianceRankings = athAllianceRankingWakeups
            //     .SelectMany(t => t.Wakeup.AthAllianceRankings
            //         .Select(src => (t.CollectedAt, AthAllianceRanking: src)));

            foreach (var t in playerRankings)
            {
                playerAggregates.Add(mapper.Map<PlayerAggregate>(t.PlayerRank, opt =>
                {
                    opt.Items[ResolutionContextKeys.PLAYER_RANKING_TYPE] = PlayerRankingType;
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }

            foreach (var t in pvpRankings)
            {
                playerAggregates.Add(mapper.Map<PlayerAggregate>(t.PvpRank, opt =>
                {
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }

            foreach (var t in allianceRankings)
            {
                playerAggregates.Add(mapper.Map<PlayerAggregate>(t.AllianceRank.Leader, opt =>
                {
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }

            foreach (var t in alliancesMembers)
            {
                playerAggregates.Add(mapper.Map<PlayerAggregate>(t,
                    opt => { opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id; }));
            }

            foreach (var t in allianceRankings)
            {
                allianceAggregates.Add(mapper.Map<AllianceAggregate>(t.AllianceRank, opt =>
                {
                    opt.Items[ResolutionContextKeys.ALLIANCE_RANKING_TYPE] = AllianceRankingType;
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }

            foreach (var t in pvpRankings.Where(t => t.PvpRank.Alliance != null))
            {
                allianceAggregates.Add(mapper.Map<AllianceAggregate>(t.PvpRank, opt =>
                {
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }

            foreach (var t in alliances)
            {
                allianceAggregates.Add(mapper.Map<AllianceAggregate>(t.Alliance, opt =>
                {
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = t.CollectedAt;
                }));
            }
        }

        await ExecuteSafeAsync(() => allianceService.AddAsync(allianceAggregates), "");
        await ExecuteSafeAsync(() => playerService.AddAsync(playerAggregates), "");

        await ExecuteSafeAsync(() => allianceRankingService.AddOrUpdateRankingsAsync(allianceAggregates), "");
        await ExecuteSafeAsync(() => allianceNameHistoryService.UpdateAsync(allianceAggregates), "");
        await ExecuteSafeAsync(
            () => allianceMembersService.UpdateAsync(allianceAggregates, allConfirmedAllianceMembers), "");

        await ExecuteSafeAsync(() => playerRankingService.AddOrUpdateRankingsAsync(playerAggregates), "");
        await ExecuteSafeAsync(() => pvpRankingService.AddOrUpdateRankingsAsync(playerAggregates), "");

        await ExecuteSafeAsync(() => playerNameHistoryService.UpdateAsync(playerAggregates), "");
        await ExecuteSafeAsync(() => playerAgeHistoryService.UpdateAsync(playerAggregates), "");
        await ExecuteSafeAsync(() => playerAllianceNameHistoryService.UpdateAsync(playerAggregates), "");
    }

    private async Task ExecuteSafeAsync(Func<Task> func, string errorMessage)
    {
        try
        {
            await func();
        }
        catch (Exception e)
        {
            logger.LogError(e, errorMessage);
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
            logger.LogError(e, errorMessage);
            return fallback;
        }
    }

    private async Task<List<(DateTime CollectedAt, Wakeup Wakeup)>> GetWakeupsAsync(string partitionKey)
    {
        var rawData = await ExecuteSafeAsync(() => inGameRawDataTableRepository.GetAllAsync(partitionKey), "", []);

        var alliances = new List<(DateTime CollectedAt, Wakeup Wakeup)>();
        foreach (var rd in rawData)
        {
            try
            {
                alliances.Add((rd.CollectedAt, inGameDataParsingService.ParseWakeup(rd.Base64Data)));
            }
            catch (Exception e)
            {
            }
        }

        return alliances;
    }

    private async Task<List<(DateTime CollectedAt, PlayerRank PlayerRank)>> GetPlayerRanking(string worldId,
        DateOnly date)
    {
        var playerRankingRawData = await ExecuteSafeAsync(
            () => inGameRawDataTableRepository.GetAllAsync(
                inGameRawDataTablePartitionKeyProvider.PlayerRankings(worldId, date, PlayerRankingType)),
            $"Error getting player raw data for world {worldId} on {date}", []);

        var rankings = new List<(DateTime CollectedAt, PlayerRank PlayerRank)>();
        foreach (var rawData in playerRankingRawData)
        {
            try
            {
                var ranks = inGameDataParsingService.ParsePlayerRanking(rawData.Base64Data);
                rankings.AddRange(ranks.Top100.Select(pr => (rawData.CollectedAt, pr)));
                rankings.AddRange(ranks.SurroundingRanking.Select(pr => (rawData.CollectedAt, pr)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing player raw data collected on {date}", rawData.CollectedAt);
            }
        }

        return rankings;
    }

    private async Task<List<(DateTime CollectedAt, AllianceRank AllianceRank)>> GetAllianceRanking(string worldId,
        DateOnly date)
    {
        var allianceRankingRawData = await ExecuteSafeAsync(
            () => inGameRawDataTableRepository.GetAllAsync(
                inGameRawDataTablePartitionKeyProvider.AllianceRankings(worldId, date, AllianceRankingType)), "", []);

        var rankings = new List<(DateTime CollectedAt, AllianceRank AllianceRank)>();
        foreach (var rawData in allianceRankingRawData)
        {
            try
            {
                var ranks = inGameDataParsingService.ParseAllianceRankings(rawData.Base64Data);
                rankings.AddRange(ranks.Top100.Select(pr => (rawData.CollectedAt, pr)));
                rankings.AddRange(ranks.SurroundingRanking.Select(pr => (rawData.CollectedAt, pr)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing alliance raw data collected on {date}", rawData.CollectedAt);
            }
        }

        return rankings;
    }

    private async Task<List<(DateTime CollectedAt, PvpRank PvpRank)>> GetPvpRanking(string worldId, DateOnly date)
    {
        var pvpRankingRawData = await ExecuteSafeAsync(
            () => inGameRawDataTableRepository.GetAllAsync(
                inGameRawDataTablePartitionKeyProvider.PvpRankings(worldId, date)), "", []);

        var rankings = new List<(DateTime CollectedAt, PvpRank PvpRank)>();
        foreach (var rawData in pvpRankingRawData)
        {
            try
            {
                rankings.AddRange(inGameDataParsingService.ParsePvpRanking(rawData.Base64Data)
                    .Select(pr => (rawData.CollectedAt, pr)));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error parsing pvp raw data collected on {date}", rawData.CollectedAt);
            }
        }

        return rankings;
    }
}
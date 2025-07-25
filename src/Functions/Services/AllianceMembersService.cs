using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceMembersService
{
    Task UpdateAsync(IEnumerable<AllianceAggregate> allianceAggregates,
        IEnumerable<(DateTime CollectedAt, AllianceKey AllianceKey, IEnumerable<int> Members)> confirmedMembers);
}

public class AllianceMembersService(IFogDbContext context, ILogger<AllianceMembersService> logger)
    : IAllianceMembersService
{
    public async Task UpdateAsync(IEnumerable<AllianceAggregate> allianceAggregates,
        IEnumerable<(DateTime CollectedAt, AllianceKey AllianceKey, IEnumerable<int> Members)> confirmedMembers)
    {
        var members = new List<(DateTime CollectedAt, AllianceKey AllianceKey, int MemberInGameId)>();
        var confirmedUpdateDates = new Dictionary<AllianceKey, DateTime>();
        var confirmedGroups = confirmedMembers
            .GroupBy(t => t.AllianceKey)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(t => t.CollectedAt)
                    .ToList());
        foreach (var chunk in confirmedGroups.Chunk(1000))
        {
            logger.LogInformation("Processing confirmed groups chunk with {GroupCount} alliance groups.", chunk.Length);

            var inGamePlayerIds = chunk.SelectMany(kvp => kvp.Value.SelectMany(t => t.Members)).ToHashSet();
            var inGameAllianceIds = chunk.Select(kvp => kvp.Key.InGameAllianceId).ToHashSet();

            logger.LogInformation("Fetching existing players for {PlayerCount} player IDs.", inGamePlayerIds.Count);
            var existingPlayers = await GetExistingPlayers(inGamePlayerIds);
            logger.LogInformation("Fetching existing alliances for {AllianceCount} alliance IDs.", inGameAllianceIds.Count);
            var existingAlliances = await GetExistingAlliances(inGameAllianceIds);

            logger.LogInformation("Fetched {PlayersFetched} players and {AlliancesFetched} alliances for current chunk.", 
                existingPlayers.Count, existingAlliances.Count);

            foreach (var allianceGroup in chunk)
            {
                if (!existingAlliances.TryGetValue(allianceGroup.Key, out var existingAlliance))
                {
                    logger.LogWarning("Alliance with key {AllianceKey} not found in existing alliances.", allianceGroup.Key);
                    continue;
                }

                var isFirst = true;
                foreach (var t in allianceGroup.Value)
                {
                    if (isFirst)
                    {
                        existingAlliance.Members.Clear();
                        foreach (var memberId in t.Members)
                        {
                            var memberKey = new PlayerKey(t.AllianceKey.WorldId, memberId);
                            if (!existingPlayers.TryGetValue(memberKey, out var existingPlayer))
                            {
                                logger.LogWarning("Player with key {PlayerKey} not found when processing alliance {AllianceKey}.", memberKey, t.AllianceKey);
                                continue;
                            }

                            existingAlliance.Members.Add(existingPlayer);
                        }

                        confirmedUpdateDates[t.AllianceKey] = t.CollectedAt;
                        isFirst = false;
                    }

                    members.AddRange(t.Members.Select(m => (t.CollectedAt, t.AllianceKey, m)));
                }
            }

            logger.LogInformation("Saving changes for confirmed groups chunk.");
            await context.SaveChangesAsync();
            logger.LogInformation("Saved changes for confirmed groups chunk successfully.");
        }

        var allianceAggregatesList = allianceAggregates.ToList();
        logger.LogInformation("Converted alliance aggregates to list with {Count} items.", allianceAggregatesList.Count);
        var leaders = allianceAggregatesList.Where(p => p.CanBeConvertedToLeader())
            .Select(a => (a.CollectedAt, AllianceKey: a.Key, MemberInGameId: a.LeaderInGameId!.Value));
        var filtered = allianceAggregatesList.Where(p => p.CanBeConvertedToMember()).ToList();
        var grouped = filtered
            .Select(a => (a.CollectedAt, AllianceKey: a.Key, MemberInGameId: a.MemberInGameId!.Value))
            .Concat(members)
            .Concat(leaders)
            .GroupBy(t => new PlayerKey(t.AllianceKey.WorldId, t.MemberInGameId))
            .ToDictionary(
                g => g.Key,
                g => g
                    .DistinctBy(t => t.AllianceKey)
                    .OrderBy(a => a.CollectedAt)
                    .ToList());
        logger.LogInformation("Aggregated player groups count: {GroupedCount}.", grouped.Count);

        foreach (var chunk in grouped.Chunk(1000))
        {
            logger.LogInformation("Processing player groups chunk with {ChunkCount} groups.", chunk.Length);

            var inGamePlayerIds = chunk.Select(kvp => kvp.Key.InGamePlayerId).ToHashSet();
            var inGameAllianceIds = chunk.SelectMany(kvp => kvp.Value.Select(t => t.AllianceKey.InGameAllianceId))
                .ToHashSet();

            logger.LogInformation("Fetching existing players for {PlayerCount} player IDs.", inGamePlayerIds.Count);
            var existingPlayers = await GetExistingPlayers(inGamePlayerIds);
            logger.LogInformation("Fetching existing alliances for {AllianceCount} alliance IDs.", inGameAllianceIds.Count);
            var existingAlliances = await GetExistingAlliances(inGameAllianceIds);
            logger.LogInformation("Fetched {PlayersFetched} players and {AlliancesFetched} alliances for player groups chunk.", 
                existingPlayers.Count, existingAlliances.Count);

            foreach (var playerGroup in chunk)
            {
                if (!existingPlayers.TryGetValue(playerGroup.Key, out var existingPlayer))
                {
                    logger.LogWarning("Player with key {PlayerKey} not found in existing players.", playerGroup.Key);
                    continue;
                }

                foreach (var t in playerGroup.Value)
                {
                    if (!existingAlliances.TryGetValue(t.AllianceKey, out var existingAlliance))
                    {
                        logger.LogWarning("Alliance with key {AllianceKey} not found in existing alliances while processing player {PlayerKey}.", t.AllianceKey, playerGroup.Key);
                        continue;
                    }

                    if (existingPlayer.AllianceHistory.All(a => a.Id != existingAlliance.Id))
                    {
                        existingPlayer.AllianceHistory.Add(existingAlliance);
                        existingPlayer.IsPresentInGame = true;
                    }

                    if (!confirmedUpdateDates.TryGetValue(t.AllianceKey, out var confirmedUpdateDate) ||
                        t.CollectedAt > confirmedUpdateDate)
                    {
                        existingPlayer.CurrentAlliance = existingAlliance;
                        existingPlayer.IsPresentInGame = true;
                    }
                }
            }

            logger.LogInformation("Saving changes for player groups chunk.");
            await context.SaveChangesAsync();
            logger.LogInformation("Saved changes for player groups chunk successfully.");
        }

        logger.LogInformation("UpdateAsync completed successfully.");
    }

    private async Task<Dictionary<PlayerKey, Player>> GetExistingPlayers(HashSet<int> inGamePlayerIds)
    {
        logger.LogInformation("Fetching existing players for {PlayerCount} player IDs.", inGamePlayerIds.Count);
        var players = await context.Players
            .Include(p => p.CurrentAlliance)
            .Include(p => p.AllianceHistory)
            .AsSplitQuery()
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();

        logger.LogInformation("Fetched {Count} players from database.", players.Count);
        return players.ToDictionary(p => p.Key);
    }

    private async Task<Dictionary<AllianceKey, Alliance>> GetExistingAlliances(HashSet<int> inGameAllianceIds)
    {
        logger.LogInformation("Fetching existing alliances for {AllianceCount} alliance IDs.", inGameAllianceIds.Count);
        var alliances = await context.Alliances
            .Include(p => p.Members)
            .Where(a => inGameAllianceIds.Contains(a.InGameAllianceId))
            .ToListAsync();

        logger.LogInformation("Fetched {Count} alliances from database.", alliances.Count);
        return alliances.ToDictionary(a => a.Key);
    }
}

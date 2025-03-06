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
            var inGamePlayerIds = chunk.SelectMany(kvp => kvp.Value.SelectMany(t => t.Members)).ToHashSet();
            var inGameAllianceIds = chunk.Select(kvp => kvp.Key.InGameAllianceId).ToHashSet();
            var existingPlayers = await GetExistingPlayers(inGamePlayerIds);
            var existingAlliances = await GetExistingAlliances(inGameAllianceIds);

            foreach (var allianceGroup in chunk)
            {
                if (!existingAlliances.TryGetValue(allianceGroup.Key, out var existingAlliance))
                {
                    // log warning
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
                                // log warning
                                continue;
                            }

                            existingAlliance.Members.Add(existingPlayer);
                            existingPlayer.AllianceName = existingAlliance.Name;
                        }

                        confirmedUpdateDates[t.AllianceKey] = t.CollectedAt;
                    }

                    members.AddRange(t.Members.Select(m => (t.CollectedAt, t.AllianceKey, m)));

                    isFirst = false;
                }
            }

            await context.SaveChangesAsync();
        }

        var list = allianceAggregates.ToList();
        var leaders = list.Where(p => p.CanBeConvertedToLeader())
            .Select(a => (a.CollectedAt, AllianceKey: a.Key, MemberInGameId: a.LeaderInGameId!.Value));
        var filtered = list.Where(p => p.CanBeConvertedToMember()).ToList();
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

        foreach (var chunk in grouped.Chunk(1000))
        {
            var inGamePlayerIds = chunk.Select(kvp => kvp.Key.InGamePlayerId).ToHashSet();
            var inGameAllianceIds = chunk.SelectMany(kvp => kvp.Value.Select(t => t.AllianceKey.InGameAllianceId))
                .ToHashSet();
            var existingPlayers = await GetExistingPlayers(inGamePlayerIds);
            var existingAlliances = await GetExistingAlliances(inGameAllianceIds);
            foreach (var playerGroup in chunk)
            {
                if (!existingPlayers.TryGetValue(playerGroup.Key, out var existingPlayer))
                {
                    // log warning
                    continue;
                }

                foreach (var t in playerGroup.Value)
                {
                    if (!existingAlliances.TryGetValue(t.AllianceKey, out var existingAlliance))
                    {
                        // log warning
                        continue;
                    }

                    if (existingPlayer.AllianceHistory.All(a => a.Id != existingAlliance.Id))
                    {
                        existingPlayer.AllianceHistory.Add(existingAlliance);
                    }

                    if (!confirmedUpdateDates.TryGetValue(t.AllianceKey, out var confirmedUpdateDate) ||
                        t.CollectedAt > confirmedUpdateDate)
                    {
                        existingPlayer.CurrentAlliance = existingAlliance;
                        existingPlayer.AllianceName = existingAlliance.Name;
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }

    private async Task<Dictionary<PlayerKey, Player>> GetExistingPlayers(HashSet<int> inGamePlayerIds)
    {
        var players = await context.Players
            .Include(p => p.CurrentAlliance)
            .Include(p => p.AllianceHistory)
            .AsSplitQuery()
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();

        return players.ToDictionary(p => p.Key);
    }

    private async Task<Dictionary<AllianceKey, Alliance>> GetExistingAlliances(HashSet<int> inGameAllianceIds)
    {
        var alliances = await context.Alliances
            .Include(p => p.Members)
            .Where(a => inGameAllianceIds.Contains(a.InGameAllianceId))
            .ToListAsync();

        return alliances.ToDictionary(a => a.Key);
    }
}
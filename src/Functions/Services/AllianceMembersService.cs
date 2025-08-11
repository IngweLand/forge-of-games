using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceMembersService
{
    Task UpdateAsync(
        IEnumerable<(DateTime CollectedAt, AllianceKey AllianceKey, IReadOnlyCollection<AllianceMember> Members)>
            confirmedMembers);
}

public class AllianceMembersService(
    IFogDbContext context,
    IFogAllianceService fogAllianceService,
    IFogPlayerService playerService,
    ILogger<AllianceMembersService> logger,
    IResultLogger resultLogger)
    : IAllianceMembersService
{
    public async Task UpdateAsync(
        IEnumerable<(DateTime CollectedAt, AllianceKey AllianceKey, IReadOnlyCollection<AllianceMember> Members)>
            confirmedMembers)
    {
        Result.Setup(cfg => { cfg.Logger = resultLogger; });
        var confirmedMembersList = confirmedMembers.ToList();
        var latestUnique = confirmedMembersList
            .GroupBy(t => t.AllianceKey)
            .Select(g => g.OrderByDescending(t => t.CollectedAt).First())
            .ToList();
        logger.LogInformation("Found {count} latest unique alliance groups of members.", latestUnique.Count);
        foreach (var alliance in latestUnique)
        {
            logger.LogInformation("Processing alliance {@allianceKey}", alliance.AllianceKey);
            var updateResult = await playerService.UpsertPlayersAsync(alliance.AllianceKey.WorldId, alliance.Members)
                .Bind(players =>
                {
                    var membersWithPlayers = alliance.Members.Select(x => (x, players.First(y => y.Id == x.Player.Id)))
                        .ToList();
                    return fogAllianceService.UpdateMembersAsync(alliance.AllianceKey, membersWithPlayers,
                        alliance.CollectedAt);
                });

            updateResult.LogIfFailed<AllianceMembersService>();
        }

        await UpdateMemberHistory(confirmedMembersList);

        logger.LogInformation("UpdateAsync completed successfully.");
    }

    private async Task UpdateMemberHistory(
        List<(DateTime CollectedAt, AllianceKey AllianceKey, IReadOnlyCollection<AllianceMember> Members)>
            confirmedMembers)
    {
        logger.LogInformation("Starting alliance member history update.");
        var unique = confirmedMembers.GroupBy(t => t.AllianceKey).ToList();
        var inGameAllianceIds = unique.Select(x => x.Key.InGameAllianceId).ToHashSet();
        logger.LogDebug("Fetching existing alliances for {AllianceCount} alliance IDs.",
            inGameAllianceIds.Count);
        var existingAlliances = await GetExistingAlliances(inGameAllianceIds);
        logger.LogDebug("Fetched {AlliancesFetched} alliances.", existingAlliances.Count);
        foreach (var group in unique)
        {
            if (!existingAlliances.TryGetValue(group.Key, out var existingAlliance))
            {
                logger.LogWarning("Alliance with key {@AllianceKey} not found in existing alliances.", group.Key);
                continue;
            }

            var uniqueMemberIds = group.SelectMany(x => x.Members).Select(x => x.Player.Id).ToHashSet();
            logger.LogDebug("Found {PlayerCount} unique members.", uniqueMemberIds.Count);
            var existingPlayers = await GetExistingPlayers(uniqueMemberIds);
            logger.LogDebug("Fetched {PlayersFetched} players.", existingPlayers.Count);
            var allianceMemberIds = existingAlliance.MemberHistory.Select(x => x.Id).ToHashSet();
            var newMembers = uniqueMemberIds.Except(allianceMemberIds).ToList();
            foreach (var memberId in newMembers)
            {
                var memberKey = new PlayerKey(existingAlliance.WorldId, memberId);
                if (!existingPlayers.TryGetValue(memberKey, out var existingPlayer))
                {
                    logger.LogWarning("Player with key {@PlayerKey} not found when processing alliance {@AllianceKey}.",
                        memberKey, existingAlliance.Key);
                    continue;
                }

                existingAlliance.MemberHistory.Add(existingPlayer);
                logger.LogDebug("Added new member to the history: {@playerKey}.", existingPlayer.Key);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Updated member history for alliance {@AllianceKey}.", existingAlliance.Key);
        }

        logger.LogInformation("Completed alliance member history update.");
    }

    private async Task<Dictionary<PlayerKey, Player>> GetExistingPlayers(HashSet<int> inGamePlayerIds)
    {
        var players = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();

        return players.ToDictionary(p => p.Key);
    }

    private async Task<Dictionary<AllianceKey, Alliance>> GetExistingAlliances(HashSet<int> inGameAllianceIds)
    {
        var alliances = await context.Alliances
            .Include(p => p.Members)
            .Include(x => x.MemberHistory)
            .Where(a => inGameAllianceIds.Contains(a.InGameAllianceId))
            .AsSplitQuery()
            .ToListAsync();

        return alliances.ToDictionary(a => a.Key);
    }
}

using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class FogAllianceService(IFogDbContext context, ILogger<FogAllianceService> logger) : IFogAllianceService
{
    public async Task<Result> UpdateMembersAsync(AllianceKey allianceKey,
        IReadOnlyCollection<(AllianceMember Member, Player Player)> membersWithPlayers,
        DateTime? collectedAt = null)
    {
        logger.LogDebug("Starting update of members for alliance {@key}", allianceKey);

        try
        {
            var alliance = await context.Alliances.Include(x => x.Members).FirstOrDefaultAsync(x =>
                x.WorldId == allianceKey.WorldId && x.InGameAllianceId == allianceKey.InGameAllianceId);

            if (alliance == null)
            {
                return Result.Fail(new FogAllianceNotFoundError(allianceKey));
            }

            if (collectedAt != null && alliance.MembersUpdatedAt > collectedAt.Value)
            {
                return Result.Fail(new OldAllianceDataError(allianceKey));
            }

            logger.LogDebug("Clearing existing members for alliance {@key}", allianceKey);
            alliance.Members.Clear();

            foreach (var item in membersWithPlayers)
            {
                alliance.Members.Add(new AllianceMemberEntity
                {
                    Player = item.Player,
                    Role = item.Member.Role,
                    JoinedAt = item.Member.JoinedAt,
                });

                if (alliance.MemberHistory.All(x => x.Id != item.Player.Id))
                {
                    alliance.MemberHistory.Add(item.Player);
                    logger.LogDebug("Added player {PlayerId} to member history for alliance {@key}",
                        item.Player.Id, allianceKey);
                }
            }

            var date = collectedAt ?? DateTime.UtcNow;
            alliance.MembersUpdatedAt = date;
            alliance.Status = InGameEntityStatus.Active;

            logger.LogDebug("Saving updated members for alliance {@key} with {MemberCount} members",
                allianceKey, membersWithPlayers.Count);

            await context.SaveChangesAsync();

            logger.LogDebug("Successfully updated members for alliance {@key}", allianceKey);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new AllianceMembersUpdateError(allianceKey, ex));
        }
    }

    public async Task UpsertAlliance(HohAlliance hohAlliance, string worldId, DateTime collectedAt)
    {
        await DoUpsertAlliance(context.Alliances, hohAlliance, worldId, collectedAt);

        await context.SaveChangesAsync();

        logger.LogDebug("Successfully upserted alliance {AllianceId} from world {WorldId}", hohAlliance.Id,
            worldId);
    }

    public async Task<Result> UpsertAlliance(HohAllianceExtended hohAlliance, string worldId, DateTime collectedAt,
        AllianceRankingType rankingType)
    {
        try
        {
            var collectedAtDate = collectedAt.ToDateOnly();
            var initQuery = context.Alliances
                .Include(p => p.Rankings.Where(x => x.CollectedAt == collectedAtDate && x.Type == rankingType));
            var alliance = await DoUpsertAlliance(initQuery, hohAlliance, worldId, collectedAt);

            if (collectedAtDate >= alliance.UpdatedAt)
            {
                alliance.Rank = hohAlliance.Rank;

                var existingRanking = alliance.Rankings
                    .FirstOrDefault(x => x.CollectedAt == collectedAtDate && x.Type == rankingType);
                if (existingRanking != null)
                {
                    existingRanking.Rank = hohAlliance.Rank;
                }
                else
                {
                    alliance.Rankings.Add(new AllianceRanking
                    {
                        Rank = hohAlliance.Rank,
                        CollectedAt = collectedAtDate,
                        Type = rankingType,
                        Points = 0, // we do not have points in extended alliance
                    });
                }
            }

            await context.SaveChangesAsync();

            logger.LogDebug("Successfully upserted alliance {AllianceId} from world {WorldId}", hohAlliance.Id,
                worldId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new AllianceUpdateError(new AllianceKey(worldId, hohAlliance.Id), ex));
        }
    }

    public async Task<Result> UpdateRanking(int allianceId, int rankingPoints, DateOnly collectedAt, int? rank = null)
    {
        var alliance = await context.Alliances
            .Include(x =>
                x.Rankings.Where(pr => pr.Type == AllianceRankingType.MemberTotal && pr.CollectedAt == collectedAt))
            .FirstOrDefaultAsync(x => x.Id == allianceId);

        if (alliance == null)
        {
            return Result.Fail(new FogAllianceNotFoundError(allianceId));
        }

        if (collectedAt >= alliance.UpdatedAt)
        {
            alliance.RankingPoints = rankingPoints;
            if (rank.HasValue)
            {
                alliance.Rank = rank.Value;
            }
        }

        var existingRanking = alliance.Rankings.LastOrDefault();
        if (existingRanking != null)
        {
            existingRanking.Points = rankingPoints;
            if (rank.HasValue)
            {
                existingRanking.Rank = rank.Value;
            }
        }
        else
        {
            alliance.Rankings.Add(new AllianceRanking
            {
                Rank = rank ?? 0,
                Points = rankingPoints,
                Type = AllianceRankingType.MemberTotal,
                CollectedAt = collectedAt,
            });
        }

        var saveResult = await Result.Try(() => context.SaveChangesAsync());
        return saveResult.ToResult();
    }

    public async Task<Result> SetAllianceMissingStatus(int allianceId, CancellationToken ct)
    {
        try
        {
            var allianceWithMembers = await context.Alliances
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == allianceId, ct);

            if (allianceWithMembers == null)
            {
                return Result.Fail(new AllianceStatusUpdateError(allianceId));
            }

            allianceWithMembers.Status = InGameEntityStatus.Missing;
            allianceWithMembers.Members.Clear();
            await context.SaveChangesAsync(ct);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new AllianceStatusUpdateError(allianceId, e));
        }
    }

    private async Task<Alliance> DoUpsertAlliance(IQueryable<Alliance> initQuery, HohAlliance hohAlliance,
        string worldId, DateTime collectedAt)
    {
        logger.LogDebug("Upserting alliance {AllianceId} from world {WorldId}",
            hohAlliance.Id, worldId);
        var existingAlliance = await initQuery
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == hohAlliance.Id && x.WorldId == worldId);

        Alliance alliance;
        if (existingAlliance == null)
        {
            logger.LogDebug("Creating new alliance {AllianceId} from world {WorldId}", hohAlliance.Id, worldId);
            alliance = new Alliance
            {
                WorldId = worldId,
                InGameAllianceId = hohAlliance.Id,
                Name = hohAlliance.Name,
            };

            context.Alliances.Add(alliance);
        }
        else
        {
            logger.LogDebug("Updating existing alliance {AllianceId} with name {AllianceName} from world {WorldId}",
                hohAlliance.Id, hohAlliance.Name, worldId);
            alliance = existingAlliance;
        }

        var collectedAtDate = collectedAt.ToDateOnly();
        if (collectedAtDate >= alliance.UpdatedAt)
        {
            alliance.Name = hohAlliance.Name;
            alliance.AvatarIconId = hohAlliance.AvatarIconId;
            alliance.AvatarBackgroundId = hohAlliance.AvatarBackgroundId;
            alliance.UpdatedAt = collectedAtDate;

            if (alliance.NameHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Name != hohAlliance.Name)
            {
                logger.LogDebug("Adding name {AllianceName} to history for alliance {AllianceId}", hohAlliance.Name,
                    hohAlliance.Id);
                alliance.NameHistory.Add(
                    new AllianceNameHistoryEntry {Name = hohAlliance.Name, ChangedAt = collectedAt});
            }
        }

        return alliance;
    }
}

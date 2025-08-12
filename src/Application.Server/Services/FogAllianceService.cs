using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
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

            logger.LogDebug("Clearing existing members for alliance {@key}",allianceKey);
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
            alliance.RankingPoints = membersWithPlayers.Sum(x => x.Member.RankingPoints);
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
    
    public async Task UpsertAlliance(HohAlliance hohAlliance, string worldId, DateTime now)
    {
        logger.LogDebug("Upserting alliance {AllianceId} from world {WorldId}",
            hohAlliance.Id, worldId);
        var existingAlliance = await context.Alliances
            .Include(p => p.NameHistory)
            .FirstOrDefaultAsync(x => x.InGameAllianceId == hohAlliance.Id && x.WorldId == worldId);

        Alliance alliance;
        if (existingAlliance != null)
        {
            logger.LogDebug("Updating existing alliance {AllianceId} with name {AllianceName} from world {WorldId}",
                hohAlliance.Id, hohAlliance.Name, worldId);
            existingAlliance.Name = hohAlliance.Name;

            alliance = existingAlliance;
        }
        else
        {
            logger.LogDebug("Creating new alliance {AllianceId} from world {WorldId}", hohAlliance.Id, worldId);
            var newAlliance = new Alliance
            {
                WorldId = worldId,
                InGameAllianceId = hohAlliance.Id,
                Name = hohAlliance.Name,
            };

            alliance = newAlliance;
            context.Alliances.Add(newAlliance);
        }

        alliance.UpdatedAt = now.ToDateOnly();
        alliance.AvatarIconId = hohAlliance.AvatarIconId;
        alliance.AvatarBackgroundId = hohAlliance.AvatarBackgroundId;

        if (alliance.NameHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.Name != hohAlliance.Name)
        {
            logger.LogDebug("Adding name {AllianceName} to history for alliance {AllianceId}", hohAlliance.Name,
                hohAlliance.Id);
            alliance.NameHistory.Add(new AllianceNameHistoryEntry {Name = hohAlliance.Name, ChangedAt = now});
        }

        logger.LogDebug("Successfully upserted alliance {AllianceId} from world {WorldId}", hohAlliance.Id,
            worldId);

        await context.SaveChangesAsync();
    }
}

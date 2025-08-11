using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class FogAllianceService(IFogDbContext context, ILogger<FogAllianceService> logger) : IFogAllianceService
{
    public async Task<Result> UpdateMembersAsync(AllianceKey allianceKey,
        IReadOnlyCollection<(AllianceMember Member, Player Player)> membersWithPlayers,
        DateTime? collectedAt = null)
    {
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
                }
            }

            var date = collectedAt ?? DateTime.UtcNow;
            alliance.RankingPoints = membersWithPlayers.Sum(x => x.Member.RankingPoints);
            alliance.MembersUpdatedAt = date;
            alliance.Status = InGameEntityStatus.Active;

            await context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new AllianceMembersUpdateError(allianceKey, ex));
        }
    }
}

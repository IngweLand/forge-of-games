using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IFogAllianceService
{
    Task<Result> UpdateMembersAsync(AllianceKey allianceKey,
        IReadOnlyCollection<(AllianceMember Member, Player Player)> membersWithPlayers,
        DateTime? collectedAt = null);

    Task UpsertAlliance(HohAlliance hohAlliance, string worldId, DateTime now);
    Task<Result> UpdateRanking(int allianceId, int rankingPoints, DateOnly collectedAt, int? rank = null);
}

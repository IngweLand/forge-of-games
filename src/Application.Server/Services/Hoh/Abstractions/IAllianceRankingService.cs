using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IAllianceRankingService
{
    Task AddOrUpdateRangeAsync(IEnumerable<AllianceRank> rankings, string worldId, DateOnly collectedAt,
        AllianceRankingType allianceRankingType);
}
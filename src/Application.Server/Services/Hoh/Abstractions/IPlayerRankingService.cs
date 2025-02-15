using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IPlayerRankingService
{
    Task AddOrUpdateRangeAsync(IEnumerable<PlayerRank> rankings, string worldId, DateOnly collectedAt,
        PlayerRankingType playerRankingType);
}

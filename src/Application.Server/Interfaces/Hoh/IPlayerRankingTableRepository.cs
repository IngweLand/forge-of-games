using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IPlayerRankingTableRepository
{
    Task SaveAsync(IEnumerable<PlayerRank> rankings, string worldId, PlayerRankingType rankingType, DateOnly date);
    Task<IReadOnlyCollection<PlayerRank>> GetAllAsync(string worldId, PlayerRankingType rankingType, DateOnly date);
}

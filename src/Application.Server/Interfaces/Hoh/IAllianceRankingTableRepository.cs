using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IAllianceRankingTableRepository
{
    Task SaveAsync(IEnumerable<AllianceRank> rankings, string worldId, AllianceRankingType rankingType, DateOnly date);
    Task<IReadOnlyCollection<AllianceRank>> GetAllAsync(string worldId, AllianceRankingType rankingType, DateOnly date);
}
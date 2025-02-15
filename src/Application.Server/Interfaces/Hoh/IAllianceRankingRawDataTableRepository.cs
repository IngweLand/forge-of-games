using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Interfaces.Hoh;

public interface IAllianceRankingRawDataTableRepository
{
    Task SaveAsync(AllianceRankingRawData data, string worldId, AllianceRankingType rankingType);

    Task<IReadOnlyCollection<AllianceRankingRawData>> GetAllAsync(string worldId, AllianceRankingType rankingType,
        DateOnly date);
}
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleSearchResultFactory
{
    Task<BattleSearchResult> Create(IReadOnlyCollection<BattleSummaryEntity> entities,
        IReadOnlyDictionary<byte[], int> existingStatsIds, BattleType battleType);
}

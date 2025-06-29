using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleSearchResultFactory
{
    Task<BattleSearchResult> Create(IReadOnlyCollection<BattleSummaryEntity> entities,
        IReadOnlyDictionary<byte[], int> existingStatsIds, BattleSquadSide side = BattleSquadSide.Player);
}

using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleSearchResultFactory
{
    Task<BattleSearchResult> Create(IReadOnlyCollection<BattleSummaryEntity> entities);
}

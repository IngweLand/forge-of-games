using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleSearchRequestFactory
{
    BattleSearchRequest Create(string uri);
    IReadOnlyDictionary<string, object?> CreateQueryParams(BattleSearchRequest request);

    IReadOnlyDictionary<string, object?> CreateQueryParams(string battleDefinitionId, Difficulty difficulty,
        BattleType battleType, IEnumerable<string>? unitIds, TreasureHuntEncounterMapDto? treasureHuntEncounterMap);
}

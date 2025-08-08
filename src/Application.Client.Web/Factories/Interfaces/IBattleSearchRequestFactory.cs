using System.Collections.ObjectModel;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleSearchRequestFactory
{
    BattleSearchRequest Create(string uri);
    IReadOnlyDictionary<string, object?> CreateQueryParams(BattleSearchRequest request);

    IReadOnlyDictionary<string, object?> CreateQueryParams(string battleDefinitionId, Difficulty difficulty,
        BattleType battleType, IEnumerable<string>? unitIds,
        IReadOnlyDictionary<(int difficulty, int stage), ReadOnlyDictionary<int, int>> treasureHuntEncounterMap);

    Task<string> CreateDefinitionTitleAsync(BattleSearchRequest request);

    Task<string> CreateDefinitionTitleAsync(string battleDefinitionId, BattleType battleType,
        Difficulty battleDifficulty,
        IReadOnlyDictionary<(int difficulty, int stage), ReadOnlyDictionary<int, int>> treasureHuntEncounterMap);
}

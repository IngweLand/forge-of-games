using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

public interface IBattleUnitProperties
{
    IReadOnlyCollection<string> Abilities { get; init; }
    int AbilityLevel { get; init; }
    int AscensionLevel { get; init; }
    int Level { get; init; }
    IReadOnlyCollection<StatBoost> StatBoosts { get; init; }
    string UnitId { get; init; }
    IReadOnlyDictionary<UnitStatType, float> UnitStatsOverrides { get; init; }
}

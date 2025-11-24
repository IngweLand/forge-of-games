using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

public interface IBattleUnitProperties
{
    IReadOnlyCollection<string> Abilities { get; init; }
    //TODO move back to init property once they fix the data
    int AbilityLevel { get; set; } 
    int AscensionLevel { get; init; }
    int AwakeningLevel { get; init; }
    IReadOnlyCollection<SquadEquipmentItem> Equipment { get; init; }
    int Level { get; init; }
    IReadOnlyCollection<StatBoost> StatBoosts { get; init; }
    string UnitId { get; init; }
    IReadOnlyDictionary<UnitStatType, float> UnitStatsOverrides { get; init; }
}

using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleUnitProperties : IBattleUnitProperties
{
    [ProtoMember(1)]
    public IReadOnlyCollection<string> Abilities { get; init; } = new List<string>();

    [ProtoMember(2)]
    public int AbilityLevel { get; init; }

    [ProtoMember(3)]
    public int AscensionLevel { get; init; }

    [ProtoMember(4)]
    public int Level { get; init; }

    [ProtoMember(5)]
    public IReadOnlyCollection<StatBoost> StatBoosts { get; init; } = new List<StatBoost>();

    [ProtoMember(6)]
    public required string UnitId { get; init; }

    [ProtoMember(7)]
    public IReadOnlyDictionary<UnitStatType, float> UnitStatsOverrides { get; init; } =
        new Dictionary<UnitStatType, float>();

    [ProtoMember(8)]
    public int AwakeningLevel { get; init; }
    
    [ProtoMember(9)]
    public IReadOnlyCollection<SquadEquipmentItem> Equipment { get; init; } = new List<SquadEquipmentItem>();
}

using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleUnitProperties
{
    [ProtoMember(1)]
    public IReadOnlyCollection<string> Abilities { get; set; } = new List<string>();

    [ProtoMember(2)]
    public int AbilityLevel { get; set; }

    [ProtoMember(3)]
    public int AscensionLevel { get; set; }

    [ProtoMember(4)]
    public int Level { get; set; }

    [ProtoMember(5)]
    public IReadOnlyCollection<StatBoost> StatBoosts { get; set; } = new List<StatBoost>();

    [ProtoMember(6)]
    public required string UnitId { get; set; }

    [ProtoMember(7)]
    public IReadOnlyDictionary<UnitStatType, float> UnitStatsOverrides { get; set; } =
        new Dictionary<UnitStatType, float>();
}

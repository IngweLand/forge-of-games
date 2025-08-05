using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicLevelDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BattleAbilityDto> Abilities { get; init; } = new List<BattleAbilityDto>();

    [ProtoMember(2)]
    public bool Ascension { get; init; }

    [ProtoMember(3)]
    public int AscensionLevel { get; init; }

    [ProtoMember(4)]
    public required int Level { get; init; }
}

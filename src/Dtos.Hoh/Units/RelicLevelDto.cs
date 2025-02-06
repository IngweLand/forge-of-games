using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicLevelDto
{
    [ProtoMember(1)]
    public required RelicBattleAbilityDto Ability { get; set; }

    [ProtoMember(2)]
    public int AscensionLevel { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<RelicBoostDto> Boosts { get; init; }

    [ProtoMember(4)]
    public bool IsAscension { get; init; }

    [ProtoMember(5)]
    public required int Level { get; init; }
}

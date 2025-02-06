using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class BattleAbilityLevelDto
{
    [ProtoMember(1)]
    public int Cost { get; init; }
    [ProtoMember(2)]
    public string? Description { get; init; }
    [ProtoMember(3)]

    public required IReadOnlyCollection<HeroAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<HeroAbilityDescriptionItem>();

    [ProtoMember(4)]
    public int Level { get; init; }
}

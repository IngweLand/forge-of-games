using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class HeroAbilityLevelDto
{
    [ProtoMember(1)]
    public int Cost { get; init; }
    [ProtoMember(2)]
    public string? Description { get; init; }
    [ProtoMember(3)]

    public required IReadOnlyCollection<BattleAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<BattleAbilityDescriptionItem>();

    [ProtoMember(4)]
    public int Level { get; init; }
}

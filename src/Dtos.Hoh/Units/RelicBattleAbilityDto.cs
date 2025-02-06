using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicBattleAbilityDto
{
    [ProtoMember(1)]
    public required string Description { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<HeroAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<HeroAbilityDescriptionItem>();

    [ProtoMember(3)]
    public required string Id { get; init; }
}

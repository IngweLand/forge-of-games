using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class BattleAbilityDto
{
    [ProtoMember(1)]
    public string? Description { get; init; }

    [ProtoMember(2)]

    public required IReadOnlyCollection<BattleAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<BattleAbilityDescriptionItem>();

    [ProtoMember(3)]
    public required string Id { get; init; }
}

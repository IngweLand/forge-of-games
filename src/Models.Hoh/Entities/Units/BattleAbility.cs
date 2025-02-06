using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class BattleAbility
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<HeroAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<HeroAbilityDescriptionItem>();
    [ProtoMember(2)]
    public required string DescriptionLocalizationId { get; init; }
    [ProtoMember(3)]
    public required string Id { get; init; }
}
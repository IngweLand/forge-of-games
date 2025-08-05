using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class BattleAbility
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BattleAbilityDescriptionItem> DescriptionItems { get; init; } =
        new List<BattleAbilityDescriptionItem>();
    [ProtoMember(2)]
    public string? DescriptionLocalizationId { get; init; }
    [ProtoMember(3)]
    public required string Id { get; init; }
}
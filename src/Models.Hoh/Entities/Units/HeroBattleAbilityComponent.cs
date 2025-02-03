using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroBattleAbilityComponent
{
    [ProtoMember(1)]
    public required string HeroAbilityId { get; init; }
    [ProtoMember(2)]
    public required IReadOnlyCollection<HeroBattleAbilityComponentLevel> Levels { get; set; } =
        new List<HeroBattleAbilityComponentLevel>();
}
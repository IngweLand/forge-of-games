using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class Relic
{
    [ProtoMember(1)]
    public RelicHeroEquipFilter? HeroEquipFilter { get; init; }
    [ProtoMember(2)] public required string Id { get; init; }
    [ProtoMember(3)] public required IReadOnlyCollection<RelicLevel> Levels { get; init; } = new List<RelicLevel>();
    [ProtoMember(4)]public required RelicRarityId RarityId { get; init; }
}

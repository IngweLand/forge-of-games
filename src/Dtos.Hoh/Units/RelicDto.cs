using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicDto
{
    [ProtoMember(1)]
    public HeroClassId HeroEquipFilter { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<RelicLevelDto> Levels { get; init; } = new List<RelicLevelDto>();

    [ProtoMember(4)]
    public required string Name { get; init; }

    [ProtoMember(5)]
    public required RelicRarityId RarityId { get; init; }
}

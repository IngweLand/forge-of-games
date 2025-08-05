using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class RelicDto
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public IReadOnlyCollection<RelicLevelDto> LevelData { get; init; } = new List<RelicLevelDto>();

    [ProtoMember(3)]
    public required string Name { get; init; }

    [ProtoMember(4)]
    public required RelicRarity Rarity { get; init; }
}

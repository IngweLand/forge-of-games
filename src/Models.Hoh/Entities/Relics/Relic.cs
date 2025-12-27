using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Relics;

[ProtoContract]
public class Relic
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public IReadOnlyCollection<RelicLevel> LevelData { get; init; } = new List<RelicLevel>();

    [ProtoMember(3)]
    public required RelicRarity Rarity { get; init; }
}

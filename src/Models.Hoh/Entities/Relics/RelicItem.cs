using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Relics;

[ProtoContract]
public class RelicItem
{
    [ProtoMember(1)]
    public required string DefinitionId { get; init; }
    [ProtoMember(2)]
    public string? EquippedOnHero { get; init; }
    [ProtoMember(3)]
    public required int Id { get; init; }
    [ProtoMember(4)]
    public required int Level { get; init; }
}

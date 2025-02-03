using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class HeroBasicDto
{
    [ProtoMember(1)]
    public required string AssetId { get; set; }

    [ProtoMember(2)]
    public required string Id { get; set; }

    [ProtoMember(3)]
    public required string Name { get; set; }

    [ProtoMember(4)]
    public required UnitColor UnitColor { get; set; }

    [ProtoMember(5)]
    public required UnitType UnitType { get; set; }
}

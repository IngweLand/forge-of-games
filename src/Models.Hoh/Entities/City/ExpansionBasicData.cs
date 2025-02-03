using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
[ProtoInclude(100, typeof(Expansion))]
public class ExpansionBasicData
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public int X { get; init; }

    [ProtoMember(4)]
    public int Y { get; init; }
}

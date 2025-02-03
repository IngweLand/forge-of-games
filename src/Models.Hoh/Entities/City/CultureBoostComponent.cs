using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class CultureBoostComponent : ComponentBase
{
    [ProtoMember(1)]
    public required double Factor { get; init; }
    [ProtoMember(2)]
    public required string Id { get; init; }
}

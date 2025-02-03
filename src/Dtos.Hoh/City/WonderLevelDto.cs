using Ingweland.Fog.Models.Hoh.Entities.City;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class WonderLevelDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<WonderCrate> Cost { get; init; }
}

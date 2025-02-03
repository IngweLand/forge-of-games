using Ingweland.Fog.Models.Hoh.Entities.Units;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BuildingUnit
{
    [ProtoMember(1)]
    public required int Level { get; init; }

    [ProtoMember(2)]
    public required Unit Unit { get; init; }
}

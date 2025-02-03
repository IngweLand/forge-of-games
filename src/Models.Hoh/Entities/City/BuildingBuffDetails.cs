using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BuildingBuffDetails
{
    [ProtoMember(1)]
    public double Factor { get; init; }

    [ProtoMember(2)]
    public IList<BuildingBuffResource>? Resources { get; set; }

    [ProtoMember(3)]
    public int Value { get; init; }
}

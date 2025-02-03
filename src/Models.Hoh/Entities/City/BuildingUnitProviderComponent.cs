using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BuildingUnitProviderComponent : ComponentBase
{
    [ProtoMember(1)]
    public required BuildingUnit BuildingUnit { get; init; }
}

using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class HeroBuildingBoostComponent : ComponentBase
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<UnitStat> UnitStats { get; init; }

    [ProtoMember(3)]
    public required UnitType UnitType { get; init; }
}

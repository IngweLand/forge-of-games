using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class CityDefinition
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BuildingType> BuildMenuTypes { get; init; }
    [ProtoMember(2)]
    public required CityId Id { get; init; }
    [ProtoMember(3)]
    public required CityInitConfigs InitConfigs { get; init; }

    [ProtoMember(4)]
    public required IList<ComponentBase> Components { get; init; } = new List<ComponentBase>();
}

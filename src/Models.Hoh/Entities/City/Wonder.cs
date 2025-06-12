using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class Wonder
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }
    [ProtoMember(2)]
    public required WonderId Id { get; init; }
    [ProtoMember(3)]
    public required WonderLevelUpComponent LevelUpComponent { get; init; }

    [ProtoMember(4)]
    public required IList<ComponentBase> Components { get; init; }
}

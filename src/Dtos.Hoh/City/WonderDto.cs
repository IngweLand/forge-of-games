using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class WonderDto
{
    [ProtoMember(1)]
    public required string CityName { get; init; }

    [ProtoMember(2)]
    public required WonderId Id { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyList<WonderLevelDto> Levels { get; init; }

    [ProtoMember(4)]
    public required string WonderName { get; init; }
    
    [ProtoMember(5)]
    public required IReadOnlyCollection<ComponentBase> Components { get; init; }
}

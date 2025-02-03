using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.CommandCenter;

[ProtoContract]
public class CommandCenterDataDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BuildingDto> Barracks { get; init; }
    [ProtoMember(2)]
    public required IReadOnlyCollection<HeroDto> Heroes { get; init; }
}

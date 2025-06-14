using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.CityPlanner;

[ProtoContract]
public class NewCityDialogItemDto
{
    [ProtoMember(1)]
    public required CityId CityId { get; set; }
    [ProtoMember(2)]
    public required string CityName { get; init; }
    [ProtoMember(3)]
    public IReadOnlyCollection<WonderDto> Wonders { get; init; } = new List<WonderDto>();
}
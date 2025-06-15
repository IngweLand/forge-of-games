using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.CityPlanner;

[ProtoContract]
public class CityPlannerDataDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BuildingDto> Buildings { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<Expansion> Expansions { get; init; }

    [ProtoMember(7)]
    public IReadOnlyCollection<BuildingCustomizationDto> BuildingCustomizations { get; init; } =
        new List<BuildingCustomizationDto>();

    [ProtoMember(8)]
    public required IReadOnlyCollection<AgeDto> Ages { get; set; }

    [ProtoMember(9)]
    public required IReadOnlyCollection<WonderDto> Wonders { get; set; } = new List<WonderDto>();
    
    [ProtoMember(10)]
    public required IReadOnlyCollection<NewCityDialogItemDto> NewCityDialogItems { get; set; }
    
    [ProtoMember(11)]
    public required CityDefinition City { get; set; }
}

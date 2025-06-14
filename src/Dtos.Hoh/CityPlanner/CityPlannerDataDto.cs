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

    [ProtoMember(2)]
    public required IReadOnlyCollection<BuildingType> BuildMenuTypes { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<Expansion> Expansions { get; init; }

    [ProtoMember(4)]
    public required int ExpansionSize { get; init; }

    [ProtoMember(5)]
    public required CityId Id { get; init; }

    [ProtoMember(6)]
    public required IReadOnlyCollection<string> InitialExpansionIds { get; init; }

    [ProtoMember(7)]
    public IReadOnlyCollection<BuildingCustomizationDto> BuildingCustomizations { get; init; } =
        new List<BuildingCustomizationDto>();

    [ProtoMember(8)]
    public required IReadOnlyCollection<AgeDto> Ages { get; set; }

    [ProtoMember(9)]
    public required IReadOnlyCollection<WonderDto> Wonders { get; set; } = new List<WonderDto>();
    
    [ProtoMember(10)]
    public required IReadOnlyCollection<NewCityDialogItemDto> NewCityDialogItems { get; set; }
}

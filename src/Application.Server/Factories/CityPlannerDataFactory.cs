using AutoMapper;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Server.Factories;

public class CityPlannerDataFactory(IMapper mapper, IWonderDtoFactory wonderDtoFactory) : ICityPlannerDataFactory
{
    public CityPlannerDataDto Create(CityDefinition cityDefinition, IReadOnlyCollection<Expansion> expansions,
        IReadOnlyCollection<BuildingDto> buildings, IEnumerable<BuildingCustomization> customizations,
        IReadOnlyCollection<Age> ages, IReadOnlyCollection<Wonder> wonders)
    {
        return new CityPlannerDataDto()
        {
            Id = cityDefinition.Id,
            BuildMenuTypes = cityDefinition.BuildMenuTypes,
            ExpansionSize = cityDefinition.InitConfigs.Grid.ExpansionSize,
            InitialExpansionIds = cityDefinition.InitConfigs.Grid.Expansions.Select(e => e.Id).ToList(),
            Expansions = expansions,
            Buildings = buildings,
            BuildingCustomizations = mapper.Map<IReadOnlyList<BuildingCustomizationDto>>(customizations),
            Ages = mapper.Map<IReadOnlyList<AgeDto>>(ages),
            Wonders = wonders.Select(wonderDtoFactory.Create).ToList(),
        };
    }
}

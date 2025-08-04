using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapBuildingGroupViewModelFactory(IMapper mapper, IBuildingLevelSpecsFactory buildingLevelSpecsFactory)
    : ICityMapBuildingGroupViewModelFactory
{
    public CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, IReadOnlyCollection<BuildingDto> buildings)
    {
        var levels = buildings.Select(buildingLevelSpecsFactory.Create).OrderBy(x => x.Level).ToList();
        return Create(buildingGroup, buildingName, buildingAge, level, levels);
    }

    public CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, BuildingLevelRange levelRange)
    {
        return Create(buildingGroup, buildingName, buildingAge, level, buildingLevelSpecsFactory.Create(levelRange));
    }

    private CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, IReadOnlyCollection<BuildingLevelSpecs> levels)
    {
        return new CityMapBuildingGroupViewModel
        {
            BuildingGroup = buildingGroup,
            Name = buildingName,
            Age = mapper.Map<AgeViewModel?>(buildingAge),
            Level = level,
            Levels = levels,
        };
    }
}

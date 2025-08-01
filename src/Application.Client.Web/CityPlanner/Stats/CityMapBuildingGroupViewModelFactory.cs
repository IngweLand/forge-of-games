using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapBuildingGroupViewModelFactory(IMapper mapper, IBuildingViewModelFactory buildingViewModelFactory)
    : ICityMapBuildingGroupViewModelFactory
{
    public CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, IReadOnlyCollection<BuildingDto> buildings)
    {
        return new CityMapBuildingGroupViewModel
        {
            BuildingGroup = buildingGroup,
            Name = buildingName,
            Age = mapper.Map<AgeViewModel?>(buildingAge),
            Level = level,
            Levels = buildings.OrderBy(x => x.Level).Select(buildingViewModelFactory.Create).ToList(),
        };
    }
}

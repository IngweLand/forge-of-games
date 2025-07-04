using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapBuildingGroupViewModelFactory(IMapper mapper) : ICityMapBuildingGroupViewModelFactory
{
    public CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, BuildingLevelRange levelRange)
    {
        return new CityMapBuildingGroupViewModel()
        {
            BuildingGroup = buildingGroup,
            Name = buildingName,
            Age = mapper.Map<AgeViewModel?>(buildingAge),
            Level = level,
            LevelRange = levelRange
        };
    }
}
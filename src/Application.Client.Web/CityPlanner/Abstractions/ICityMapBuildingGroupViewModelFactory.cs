using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapBuildingGroupViewModelFactory
{
    CityMapBuildingGroupViewModel Create(BuildingGroup buildingGroup, string buildingName, AgeDto? buildingAge,
        int? level, BuildingLevelRange levelRange);
}
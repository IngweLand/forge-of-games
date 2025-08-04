using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapEntityViewModelFactory
{
    CityMapEntityViewModel Create(CityMapEntity entity, BuildingDto building,
        IReadOnlyCollection<BuildingDto> buildings, IReadOnlyCollection<BuildingCustomizationDto> customizations);

    CityMapEntityViewModel Create(CityMapEntity entity, BuildingDto building,
        BuildingLevelRange levelRange, IReadOnlyCollection<BuildingCustomizationDto> customizations);
}

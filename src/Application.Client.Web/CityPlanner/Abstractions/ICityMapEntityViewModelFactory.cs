using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapEntityViewModelFactory
{
    CityMapEntityViewModel Create(CityMapEntity entity, BuildingDto building,
        IReadOnlyCollection<BuildingDto> buildings, IReadOnlyCollection<BuildingCustomizationDto> customizations);
}

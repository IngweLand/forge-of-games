using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerCityPropertiesViewModelFactory
{
    CityPlannerCityPropertiesViewModel Create(CityId cityId, string name, AgeViewModel age, CityStats stats,
        IEnumerable<BuildingDto> buildings);
}

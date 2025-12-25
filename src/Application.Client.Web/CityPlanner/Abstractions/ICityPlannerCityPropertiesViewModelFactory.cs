using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerCityPropertiesViewModelFactory
{
    CityPlannerCityPropertiesViewModel Create(CityId inGameCityId, string cityName, AgeViewModel age,
        CityStats cityStats, IEnumerable<BuildingDto> buildingsValues, WonderDto? wonder = null, int wonderLevel = 0);
}

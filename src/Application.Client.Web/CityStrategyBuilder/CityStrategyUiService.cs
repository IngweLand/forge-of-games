using Ingweland.Fog.Application.Client.Web.CityPlanner;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

public class CityStrategyUiService(ICityStrategyFactory cityStrategyFactory) : ICityStrategyUiService
{
    public CityStrategy CreateCityStrategy(NewCityRequest newCityRequest)
    {
        return cityStrategyFactory.Create(newCityRequest, FogConstants.CITY_PLANNER_VERSION);
    }
}

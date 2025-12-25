using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlannerUiService(IHohCityFactory hohCityFactory) : ICityPlannerUiService
{
    public HohCity CreateNew(NewCityRequest newCityRequest)
    {
        return hohCityFactory.Create(newCityRequest, FogConstants.CITY_PLANNER_VERSION);
    }
}

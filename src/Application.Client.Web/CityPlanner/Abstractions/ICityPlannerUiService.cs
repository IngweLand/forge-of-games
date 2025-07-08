using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerUiService
{
    HohCity CreateNew(NewCityRequest newCityRequest);
}

using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface ICityPlannerDataService
{
    Task<CityPlannerDataDto> GetCityPlannerDataAsync(CityId cityId);
}

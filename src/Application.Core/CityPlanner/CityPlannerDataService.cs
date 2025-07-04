using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityPlannerDataService(ICityService cityService) : ICityPlannerDataService
{
    private readonly Dictionary<CityId, CityPlannerDataDto> _cityPlannerDataCache = new ();

    public async Task<CityPlannerDataDto> GetCityPlannerDataAsync(CityId cityId)
    {
        if (!_cityPlannerDataCache.TryGetValue(cityId, out var cityPlannerData))
        {
            cityPlannerData = (await cityService.GetCityPlannerDataAsync(cityId))!;
            _cityPlannerDataCache.Add(cityId, cityPlannerData);
        }

        return cityPlannerData;
    }
}

using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Caching;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityPlannerDataService : ICityPlannerDataService
{
    private readonly AsyncCache<CityId, CityPlannerDataDto> _cityPlannerDataCache;
    private readonly ICityService _cityService;

    public CityPlannerDataService(ICityService cityService)
    {
        _cityService = cityService;

        _cityPlannerDataCache =
            new AsyncCache<CityId, CityPlannerDataDto>(x => _cityService.GetCityPlannerDataAsync(x));
    }

    public Task<CityPlannerDataDto> GetCityPlannerDataAsync(CityId cityId)
    {
        return _cityPlannerDataCache.GetAsync(cityId)!;
    }

    public async Task<IReadOnlyCollection<NewCityDialogItemDto>> GetNewCityDialogItemsAsync()
    {
        if (_cityPlannerDataCache.Keys.Count == 0)
        {
            await GetCityPlannerDataAsync(CityId.Capital);
        }

        return _cityPlannerDataCache.Values.First().NewCityDialogItems;
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;

public class CityInspirationsUiService(
    ICommonUiService commonUiService,
    IPersistenceService persistenceService,
    ICityPlannerService cityPlannerService,
    IPlayerCitySnapshotViewModelFactory playerCitySnapshotViewModelFactory,
    IMapper mapper) : ICityInspirationsUiService
{
    private static readonly List<CitySnapshotSearchPreference> SearchPreferences =
        [CitySnapshotSearchPreference.Food, CitySnapshotSearchPreference.Coins, CitySnapshotSearchPreference.Goods];

    public async Task<CityInspirationsSearchFormViewModel> GetSearchFormDataAsync()
    {
        var cities = await persistenceService.GetCities();

        return new CityInspirationsSearchFormViewModel
        {
            Ages = (await commonUiService.GetAgesAsync()).Values.Where(x => x.Index is > 2 and < 14).ToList(),
            Cities = cities,
            SearchPreferences =
                mapper.Map<IReadOnlyCollection<CitySnapshotSearchPreferenceViewModel>>(SearchPreferences),
        };
    }

    public async Task<IReadOnlyCollection<PlayerCitySnapshotBasicViewModel>> GetInspirationsAsync(
        CityInspirationsSearchRequest request, CancellationToken ct = default)
    {
        var ages = await commonUiService.GetAgesAsync();
        var snapshots = await cityPlannerService.GetInspirationsAsync(request, ct);
        var orderBy = request.SearchPreference switch
        {
            CitySnapshotSearchPreference.Coins => x => x.Coins,
            CitySnapshotSearchPreference.Goods => x => x.Goods,
            _ => new Func<PlayerCitySnapshotBasicDto, int>(x => x.Food),
        };
        return snapshots.OrderByDescending(orderBy).Select(x =>
            playerCitySnapshotViewModelFactory.Create(x, ages.GetValueOrDefault(x.AgeId, AgeViewModel.Blank))).ToList();
    }

    public Task<HohCity?> GetPlayerCitySnapshotAsync(int snapshotId, CancellationToken ct = default)
    {
        return cityPlannerService.GetPlayerCitySnapshotAsync(snapshotId, ct);
    }
}

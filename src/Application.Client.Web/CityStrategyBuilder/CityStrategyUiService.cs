using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

public class CityStrategyUiService(
    ICityStrategyFactory cityStrategyFactory,
    IPlayerCityStrategyService playerCityStrategyService,
    ICommonUiService commonUiService,
    ICommunityCityStrategyService communityCityStrategyService,
    ICommunityCityStrategyViewModelFactory cityStrategyViewModelFactory,
    IFogSharingUiService fogSharingUiService,
    IPersistenceService persistenceService,
    ILogger<CityStrategyUiService> logger) : UiServiceBase(logger), ICityStrategyUiService
{
    public CityStrategy CreateCityStrategy(NewCityRequest newCityRequest)
    {
        return cityStrategyFactory.Create(newCityRequest, FogConstants.CITY_PLANNER_VERSION);
    }

    public Task<CityStrategy?> FetchPlayerStrategy(int strategyId, CancellationToken ct = default)
    {
        return ExecuteSafeAsync(() => playerCityStrategyService.GetPlayerCityStrategyAsync(strategyId, ct)!, null);
    }

    public async Task<CityStrategy?> GetCommunityStrategyAsync(string strategyId)
    {
        var strategy = await persistenceService.LoadCityStrategy(strategyId, true);
        if (strategy != null)
        {
            return strategy;
        }

        strategy = await fogSharingUiService.FetchCityStrategyAsync(strategyId);
        if (strategy != null)
        {
            await persistenceService.SaveCommunityCityStrategy(strategyId, strategy);
        }

        return strategy;
    }

    public async Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetCommunityStrategiesAsync()
    {
        var strategies = await ExecuteSafeAsync(() => communityCityStrategyService.GetStrategiesAsync(), []);
        IReadOnlyDictionary<string, AgeViewModel> ages = new Dictionary<string, AgeViewModel>();
        if (strategies.Any(x => x.AgeId != null))
        {
            ages = await commonUiService.GetAgesAsync();
        }

        return strategies.OrderByDescending(x => x.UpdatedAt)
            .Select(x => cityStrategyViewModelFactory.Create(x, ages)).ToList();
    }

    public async Task<IReadOnlyCollection<CommunityCityGuideViewModel>> GetCommunityGuidesAsync()
    {
        var guides = await ExecuteSafeAsync(() => communityCityStrategyService.GetGuidesAsync(), []);
        IReadOnlyDictionary<string, AgeViewModel> ages = new Dictionary<string, AgeViewModel>();
        if (guides.Any(x => x.AgeId != null))
        {
            ages = await commonUiService.GetAgesAsync();
        }

        return guides.OrderByDescending(x => x.UpdatedAt)
            .Select(x => cityStrategyViewModelFactory.Create(x, ages)).ToList();
    }

    public async Task<CommunityCityGuideViewModel?> GetCommunityGuideAsync(int guideId)
    {
        var guide = await ExecuteSafeAsync(() => communityCityStrategyService.GetGuideAsync(guideId), null);
        if (guide == null)
        {
            return null;
        }

        AgeViewModel? age = null;
        if (guide.AgeId != null)
        {
            age = await commonUiService.GetAgeAsync(guide.AgeId);
        }

        return cityStrategyViewModelFactory.Create(guide, age);
    }
}

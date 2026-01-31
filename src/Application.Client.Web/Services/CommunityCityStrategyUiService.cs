using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class CommunityCityStrategyUiService(
    ICommonUiService commonUiService,
    ICommunityCityStrategyService communityCityStrategyService,
    ICommunityCityStrategyViewModelFactory cityStrategyViewModelFactory,
    ILogger<CommunityCityStrategyUiService> logger) : UiServiceBase(logger), ICommunityCityStrategyUIService
{
    public async Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetStrategiesAsync()
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

    public async Task<IReadOnlyCollection<CommunityCityGuideViewModel>> GetGuidesAsync()
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

    public async Task<CommunityCityGuideViewModel?> GetGuideAsync(int guideId)
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

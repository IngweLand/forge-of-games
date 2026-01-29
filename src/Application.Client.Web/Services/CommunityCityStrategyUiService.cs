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
    ISharedCityStrategyService sharedCityStrategyService,
    ICommunityCityStrategyViewModelFactory cityStrategyViewModelFactory,
    ILogger<CommunityCityStrategyUiService> logger) : UiServiceBase(logger), ICommunityCityStrategyUIService
{
    public async Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetAllAsync()
    {
        var strategies = await ExecuteSafeAsync(() => sharedCityStrategyService.GetAllAsync(), []);
        IReadOnlyDictionary<string, AgeViewModel> ages = new Dictionary<string, AgeViewModel>();
        if (strategies.Any(x => x.AgeId != null))
        {
            ages = await commonUiService.GetAgesAsync();
        }

        return strategies.OrderByDescending(x => x.UpdatedAt)
            .Select(x => cityStrategyViewModelFactory.Create(x, ages)).ToList();
    }
}

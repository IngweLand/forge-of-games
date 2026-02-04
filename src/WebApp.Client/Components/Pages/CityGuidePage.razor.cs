using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class CityGuidePage : FogPageBase
{
    private string _content = string.Empty;
    private string _pageTitle = string.Empty;

    [Inject]
    private IAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private ICommunityCityStrategyUIService CommunityCityStrategyUiService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var guide = await LoadWithPersistenceAsync("guide",
            () => CommunityCityStrategyUiService.GetGuideAsync(GuideId));

        if (guide == null)
        {
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH, false, true);
            return;
        }

        _pageTitle = Loc[FogResource.CityGuide_PageTitle, guide.Name].ToString();

        _content = guide.Content;

        if (OperatingSystem.IsBrowser())
        {
            var analyticsParams = new Dictionary<string, object>
            {
                [AnalyticsParams.CITY_ID] = guide.CityId.ToString(),
                [AnalyticsParams.CITY_GUIDE_ID] = guide.Id,
            };
            _ = AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_GUIDE, analyticsParams);
        }
    }
}

using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class AddSharedResourcePage : FogPageBase
{
    [Inject]
    private CityStrategyNavigationState CityStrategyNavigationState { get; set; }

    [Inject]
    private IFogSharingUiService FogSharingUiService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override bool ShouldHidePageLoadingIndicator => false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var url = "/";
        if (NavigationManager.Uri == BuildUrl(FogUrlBuilder.PageRoutes.GET_SHARED_STRATEGY_TEMPLATE))
        {
            var strategy = await FogSharingUiService.FetchCityStrategyAsync(ShareId);
            if (strategy != null)
            {
                CityStrategyNavigationState.Data = new CityStrategyNavigationState.CityStrategyNavigationStateData
                {
                    Strategy = strategy,
                    IsRemote = true,
                };
                url = FogUrlBuilder.PageRoutes.CITY_STRATEGY_VIEWER_PATH;
            }
        }
        else if (NavigationManager.Uri == BuildUrl(FogUrlBuilder.PageRoutes.GET_SHARED_EQUIPMENT_PROFILE_TEMPLATE))
        {
            if (await FogSharingUiService.LoadEquipmentProfileAsync(ShareId))
            {
                url = FogUrlBuilder.PageRoutes.COMMAND_CENTER_EQUIPMENT_CONFIGURATOR_DASHBOARD_PATH;
            }
        }

        Navigate(url);
    }

    private string BuildUrl(string path)
    {
        return $"{NavigationManager.BaseUri.TrimEnd('/')}{path.Replace("{shareId}", ShareId)}";
    }

    private void Navigate(string path)
    {
        NavigationManager.NavigateTo(path, false, true);
    }
}

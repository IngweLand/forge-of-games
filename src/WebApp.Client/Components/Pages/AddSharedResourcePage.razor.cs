using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class AddSharedResourcePage : FogPageBase
{
    [Inject]
    private IFogSharingUiService FogSharingUiService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

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
            if (await FogSharingUiService.LoadCityStrategyAsync(ShareId))
            {
                url = FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH;
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

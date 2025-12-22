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

        var loaded = false;
        if (NavigationManager.Uri == BuildUrl(FogUrlBuilder.PageRoutes.GET_SHARED_STRATEGY_TEMPLATE))
        {
            loaded = await FogSharingUiService.LoadCityStrategyAsync(ShareId);
        }

        Navigate(!loaded ? "/" : FogUrlBuilder.PageRoutes.CITY_STRATEGIES_DASHBOARD_PATH);
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

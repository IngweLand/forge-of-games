using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public class StatsHubPageBase : FogPageBase, IDisposable
{
    protected bool IsInitialized { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IStatsHubUiService StatsHubUiService { get; set; }
}

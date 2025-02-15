using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.WebApp.Client.Components.Layout;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

[Layout(typeof(CommandCenterLayout))]
[StreamRendering]
public class StatsHubPageBase : ComponentBase, IDisposable
{
    protected static IComponentRenderMode PageRenderMode = new InteractiveWebAssemblyRenderMode();
    private PersistingComponentStateSubscription _persistingSubscription;

    [Inject]
    protected PersistentComponentState ApplicationState { get; set; }

    [Inject]
    protected IJSInteropService IJsInteropService { get; set; }

    protected bool IsInitialized { get; set; }

    [Inject]
    protected IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IStatsHubUiService StatsHubUiService { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _persistingSubscription.Dispose();
        }
    }

    protected override Task OnInitializedAsync()
    {
        _persistingSubscription = ApplicationState.RegisterOnPersisting(PersistData);

        return Task.CompletedTask;
    }

    protected virtual Task PersistData()
    {
        return Task.CompletedTask;
    }
}

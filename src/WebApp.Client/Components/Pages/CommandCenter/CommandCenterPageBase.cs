using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.WebApp.Client.Components.Layout;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

[Layout(typeof(CommandCenterLayout))]
public abstract class CommandCenterPageBase : ComponentBase, IDisposable
{
    protected static IComponentRenderMode PageRenderMode = new InteractiveWebAssemblyRenderMode(prerender: false);
    [Inject]
    protected ICommandCenterUiService CommandCenterUiService { get; set; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected ISnackbar Snackbar { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Snackbar.Dispose();
            if (IsInitialized)
            {
                CommandCenterUiService.StateHasChanged -= CommandCenterUiServiceOnStateHasChanged;
            }
        }
    }
    
    [Inject]
    protected IJSInteropService IjsInteropService { get; set; }

    protected static DialogOptions GetDefaultDialogOptions()
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter,
        };
    }
    protected bool IsInitialized { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected override async Task OnInitializedAsync()
    {
        IjsInteropService.ResetScrollPositionAsync();
        await CommandCenterUiService.EnsureInitializedAsync();
        await IjsInteropService.RemoveLoadingIndicatorAsync();
        CommandCenterUiService.StateHasChanged += CommandCenterUiServiceOnStateHasChanged;
        IsInitialized = true;
    }
    
    private void CommandCenterUiServiceOnStateHasChanged()
    {
       StateHasChanged();
    }
}

using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public abstract class CommandCenterPageBase : FogPageBase
{
    [Inject]
    protected ICommandCenterUiService CommandCenterUiService { get; set; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected ISnackbar Snackbar { get; set; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Snackbar.Dispose();
            if (IsInitialized)
            {
                CommandCenterUiService.StateHasChanged -= CommandCenterUiServiceOnStateHasChanged;
            }
        }
        
        base.Dispose(disposing);
    }

    protected sealed override async Task OnParametersSetAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        await HandleOnParametersSetAsync();
    }

    protected virtual Task HandleOnParametersSetAsync()
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task HandleOnInitializedAsync()
    {
        return Task.CompletedTask;
    }

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

    protected sealed override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }
        
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        JsInteropService.ResetScrollPositionAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        await CommandCenterUiService.EnsureInitializedAsync();
        CommandCenterUiService.StateHasChanged += CommandCenterUiServiceOnStateHasChanged;
        await HandleOnInitializedAsync();
        IsInitialized = true;
        await JsInteropService.HideLoadingIndicatorAsync();
    }
    
    private void CommandCenterUiServiceOnStateHasChanged()
    {
       StateHasChanged();
    }
}

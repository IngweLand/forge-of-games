using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public abstract class CcProfilePageBase : CommandCenterPageBase
{
    [Parameter]
    public required string ProfileId { get; set; }

    [Inject]
    protected ICcProfileUiService ProfileUiService { get; set; }

    protected CcProfileViewModel? Profile { get; private set; }

    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();

        var profile = await ProfileUiService.GetProfileAsync(ProfileId);
        if (profile == null)
        {
            NavigationManager.NavigateTo($"command-center/profiles", false, true);
        }
        else
        {
            Profile = profile;
            ProfileUiService.StateHasChanged += ProfileUiServiceOnStateHasChanged;
        }
    }

    private void ProfileUiServiceOnStateHasChanged()
    {
        _ = InvokeAsync(HandleProfileUiServiceOnChangeAsync);
    }

    protected virtual async Task HandleProfileUiServiceOnChangeAsync()
    {
        try 
        {
            Profile = await ProfileUiService.GetProfileAsync(ProfileId);
            StateHasChanged();
        }
        catch (Exception e)
        {
            // Handle or log the exception
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (IsInitialized)
            {
                ProfileUiService.StateHasChanged -= ProfileUiServiceOnStateHasChanged;
            }
        }
        
        base.Dispose(disposing);
    }
}

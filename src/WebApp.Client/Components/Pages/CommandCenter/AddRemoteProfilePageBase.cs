using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public abstract class AddRemoteProfilePageBase: CommandCenterPageBase
{
    protected override void OnInitialized()
    {
        if (CanGetProfile())
        {
            return;
        }

        OpenMainPage();
    }

    protected override async Task HandleOnInitializedAsync()
    {
        await base.HandleOnInitializedAsync();
        
        var sharingTask = GetProfile();
        try
        {
            await sharingTask;
        }
        catch (Exception e)
        {
            OpenMainPage();
            return;
        }
        
        if (sharingTask.Result != null)
        {
            await CreateNewProfile(sharingTask.Result);
        }
        else
        {
            OpenMainPage();
        }
    }

    protected abstract bool CanGetProfile();
    protected abstract Task<BasicCommandCenterProfile?> GetProfile();
    private void OpenMainPage()
    {
        NavigationManager.NavigateTo("command-center", false, true);
    }
    
    private async Task CreateNewProfile(BasicCommandCenterProfile profileDto)
    {
        var options = GetDefaultDialogOptions();
        var parameters = new DialogParameters<TextInputDialog>
        {
            {d => d.PositiveButtonText, Loc[FogResource.Common_Create]},
            {d => d.InputLabel, Loc[FogResource.CommandCenter_CreateNewProfileDialog_InputLabel]},
        };
        var dialog =
            await DialogService.ShowAsync<TextInputDialog>(Loc[FogResource.CommandCenter_CreateNewProfileDialog_Title],
                parameters, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            OpenMainPage();
            return;
        }

        var profileName = result.Data as string;
        if (string.IsNullOrWhiteSpace(profileName))
        {
            OpenMainPage();
            return;
        }

        var profileId = await CommandCenterUiService.CreateProfileAsync(profileName, profileDto);
        NavigationManager.NavigateTo($"command-center/profiles/{profileId}", false, true);
    }
}

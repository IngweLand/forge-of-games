using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.WebApp.Client.Components.Elements;
using Ingweland.Fog.WebApp.Client.Components.Elements.CommandCenter;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfilePage : CcProfilePageBase
{
    [Inject]
    private IAssetUrlProvider AssetUrlProvider { get; set; }

    [Inject]
    private ICommandCenterProfileSharingService CommandCenterProfileRepository { get; set; }

    private async Task CreateTeam()
    {
        var options = GetDefaultDialogOptions();
        var parameters = new DialogParameters<TextInputDialog>
        {
            {d => d.PositiveButtonText, Loc[FogResource.Common_Create]},
            {d => d.InputLabel, Loc[FogResource.CommandCenter_CreateNewTeamDialog_InputLabel]},
        };
        var dialog =
            await DialogService.ShowAsync<TextInputDialog>(Loc[FogResource.CommandCenter_CreateNewTeamDialog_Title],
                parameters, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        var profileName = result.Data as string;
        if (string.IsNullOrWhiteSpace(profileName))
        {
            return;
        }

        var teamId = await ProfileUiService.CreateTeamAsync(ProfileId, profileName);
        if (teamId == null)
        {
            return;
        }

        StateHasChanged();
    }

    private async Task DeleteTeam(string teamId)
    {
        await ProfileUiService.DeleteTeamAsync(ProfileId, teamId);
    }

    private void OpenProfileBarracks()
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/barracks");
    }

    private void OpenProfileHeroesPage()
    {
        NavigationManager.NavigateTo($"command-center/profiles/{ProfileId}/heroes");
    }

    private void OpenTeam(string teamId)
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/{teamId}");
    }

    private async Task ShareProfile()
    {
        var options = GetDefaultDialogOptions();
        var parameters = new DialogParameters<ShareCcProfileDialog>
        {
            {d => d.ProfileId, ProfileId},
        };
        await DialogService.ShowAsync<ShareCcProfileDialog>(Loc[FogResource.CommandCenter_ShareProfileDialog_Title],
            parameters, options);
    }

    private void OpenSettingsPage()
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/settings");
    }
}

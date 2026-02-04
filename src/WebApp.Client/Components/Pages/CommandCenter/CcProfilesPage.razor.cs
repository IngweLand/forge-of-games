using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.WebApp.Client.Components.Elements;
using Ingweland.Fog.WebApp.Client.Components.Elements.CityPlanner;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfilesPage : CommandCenterPageBase
{
    private IReadOnlyCollection<CcProfileBasicsViewModel> _profiles = new List<CcProfileBasicsViewModel>();

    [Inject]
    private IInGameStartupDataService InGameStartupDataService { get; set; }

    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();
        
        _profiles = await CommandCenterUiService.GetProfiles();
    }

    private async Task CreateNewProfile()
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
            return;
        }

        var profileName = result.Data as string;
        if (string.IsNullOrWhiteSpace(profileName))
        {
            return;
        }

        var profileId = await CommandCenterUiService.CreateProfileAsync(profileName);
        OpenProfile(profileId);
    }

    private void NavigateTo(string path)
    {
        NavigationManager.NavigateTo(path);
    }
    
    private void OpenProfile(string profileId)
    {
        NavigationManager.NavigateTo($"command-center/profiles/{profileId}");
    }
}

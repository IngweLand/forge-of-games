using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
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

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
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

    private async Task HandleImportData(string data)
    {
        string? error = null;
        ResourceCreatedResponse? response = null;
        try
        {
            response = await InGameStartupDataService.ImportInGameDataAsync(new ImportInGameStartupDataRequestDto()
            {
                InGameStartupData = data,
            });
        }
        catch (Exception e)
        {
            error = "Could not import data.";
        }

        if (response != null)
        {
            NavigationManager.NavigateTo(response.WebResourceUrl);
        }
        else if (error != null)
        {
            Snackbar!.Add(error, Severity.Error);
        }
    }

    private async Task ImportProfile()
    {
        var options = GetDefaultDialogOptions();
        var dialog = await DialogService.ShowAsync<ImportInGameCityDialog>(null, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        var importedText = result.Data as string;
        if (string.IsNullOrWhiteSpace(importedText))
        {
            return;
        }

        await HandleImportData(importedText);
    }

    private void OpenProfile(string profileId)
    {
        NavigationManager.NavigateTo($"command-center/profiles/{profileId}");
    }
}

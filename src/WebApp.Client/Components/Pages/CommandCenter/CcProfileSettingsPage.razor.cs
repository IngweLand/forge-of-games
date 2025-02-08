using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Core.Constants;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileSettingsPage : CcProfilePageBase
{
    private CcProfileSettings? _profileSettings;

    private IEnumerable<string> ValidateInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            yield return Loc[FogResource.CityPlanner_NameRequiredError];
        }
        else if (input.Length > FogConstants.NAME_MAX_CHARACTERS)
        {
            yield return Loc[FogResource.CityPlanner_NameTooLongError, FogConstants.NAME_MAX_CHARACTERS];
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _profileSettings = await ProfileUiService.GetSettingsAsync(ProfileId);
    }

    private async Task SaveSettings()
    {
        await ProfileUiService.UpdateProfileSettingsAsync(ProfileId, _profileSettings!);
    }

    private async Task DeleteProfile()
    {
        var success = await CommandCenterUiService.DeleteProfileAsync(ProfileId);
        if(success)
        {
            NavigationManager.NavigateTo("command-center/profiles", false, true);
        }
    }
    private async Task OnDeleteProfileClicked()
    {
        var result = await DialogService.ShowMessageBox(
            null,
            Loc[FogResource.Common_DeleteConfirmation, Profile!.Name],
            yesText: Loc[FogResource.Common_Delete], cancelText: Loc[FogResource.Common_Cancel]);
        if (result != null)
        {
            await DeleteProfile();
        }
    }
}


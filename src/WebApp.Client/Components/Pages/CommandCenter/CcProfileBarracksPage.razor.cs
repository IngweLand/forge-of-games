using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileBarracksPage : CcProfilePageBase
{
    private IReadOnlyCollection<CcBarracksViewModel>? _barracks;
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _barracks = await ProfileUiService.GetBarracks(ProfileId);
    }

    private async Task OnBarracksLevelChanged(CcBarracksViewModel barracks)
    {
        await ProfileUiService.UpdateBarracks(ProfileId, barracks);
    }
}


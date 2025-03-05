using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileBarracksPage : CcProfilePageBase
{
    private IReadOnlyCollection<CcBarracksViewModel>? _barracks;
    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();
        
        _barracks = await ProfileUiService.GetBarracks(ProfileId);
    }

    private async Task OnBarracksLevelChanged(CcBarracksViewModel barracks)
    {
        await ProfileUiService.UpdateBarracks(ProfileId, barracks);
    }
}


using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.WebApp.Client.Components.Elements.CommandCenter;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileTeamPage : CcProfilePageBase
{
    private CcProfileTeamViewModel? _team;
    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();
        
        _team = await ProfileUiService.GetTeamAsync(ProfileId, TeamId);
    }

    protected override async Task HandleProfileUiServiceOnChangeAsync()
    {
        await base.HandleProfileUiServiceOnChangeAsync();
        _team = await ProfileUiService.GetTeamAsync(ProfileId, TeamId);
        StateHasChanged();
    }

    private async Task AddHero()
    {
        var heroes = await ProfileUiService.GetAddableHeroesForTeamAsync(ProfileId, TeamId);
        if (heroes.Count == 0)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<CcAddHeroDialog> {{d => d.Heroes, heroes}};
        var dialog = await DialogService.ShowAsync<CcAddHeroDialog>(null, parameters, options);
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }

        var heroId = result.Data as string;
        if (string.IsNullOrWhiteSpace(heroId))
        {
            return;
        }

        await ProfileUiService.AddHeroToTeamAsync(ProfileId, TeamId, heroId);
    }

    private void OnHeroProfileSelected(string heroProfileId)
    {
        NavigationManager.NavigateTo($"/command-center/profiles/{ProfileId}/heroes/{heroProfileId}"); 
    }

    private async Task RemoveHero(string heroProfileId)
    {
        await ProfileUiService.RemoveHeroFromTeamAsync(ProfileId, TeamId, heroProfileId);
    }
}


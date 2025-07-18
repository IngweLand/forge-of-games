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

        _team = ProfileUiService.GetTeam(TeamId);
    }

    protected override async Task HandleProfileUiServiceOnChangeAsync()
    {
        await base.HandleProfileUiServiceOnChangeAsync();
        _team = ProfileUiService.GetTeam(TeamId);
        StateHasChanged();
    }

    private async Task AddHero()
    {
        var heroes = ProfileUiService.GetAddableHeroesForTeam(TeamId);
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

        await ProfileUiService.AddHeroToTeamAsync(TeamId, heroId);
    }

    private void OnHeroProfileSelected(string heroId)
    {
        NavigationManager.NavigateTo($"/command-center/profiles/{ProfileId}/heroes/{heroId}");
    }

    private async Task RemoveHero(string heroId)
    {
        await ProfileUiService.RemoveHeroFromTeamAsync(TeamId, heroId);
    }
}

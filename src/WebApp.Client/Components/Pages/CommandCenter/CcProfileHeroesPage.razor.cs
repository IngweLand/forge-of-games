using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.WebApp.Client.Components.Elements.CommandCenter;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileHeroesPage : CcProfilePageBase
{
    private IEnumerable<HeroProfileViewModel>? _heroes;
    private string? _profileName;

    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();

        _profileName = ProfileUiService.GetCurrentProfile().Name;
        await GetHeroes();
    }

    private async Task AddHero()
    {
        var heroes = ProfileUiService.GetAddableHeroesForProfile();
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

        await ProfileUiService.AddHeroAsync(heroId);
    }

    private async Task GetHeroes()
    {
        var heroes = await ProfileUiService.GetProfileHeroesAsync();
        _heroes = heroes.OrderByDescending(h => h.TotalPower);
    }

    protected override async Task HandleProfileUiServiceOnChangeAsync()
    {
        await base.HandleProfileUiServiceOnChangeAsync();
        await GetHeroes();
        StateHasChanged();
    }

    private void OnHeroProfileSelected(string heroProfileId)
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/{heroProfileId}");
    }
}

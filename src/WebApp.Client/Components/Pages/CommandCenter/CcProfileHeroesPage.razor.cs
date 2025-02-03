using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.WebApp.Client.Components.Elements.CommandCenter;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcProfileHeroesPage :CcProfilePageBase
{

    private IEnumerable<HeroProfileViewModel>? _heroes;
    private string? _profileName;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _profileName = (await ProfileUiService.GetProfileAsync(ProfileId))?.Name;
        await GetHeroes();
    }

    private async Task AddHero()
    {
        var heroes = await ProfileUiService.GetAddableHeroesForProfileAsync(ProfileId);
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

        await ProfileUiService.AddHeroAsync(ProfileId, heroId);
    }
    
    private async Task GetHeroes()
    {
        var heroes = await ProfileUiService.GetProfileHeroesAsync(ProfileId);
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

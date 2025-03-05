using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcPlaygroundHeroesPage : CommandCenterPageBase
{
    [Inject]
    private ICcHeroesPlaygroundUiService PlaygroundUiService { get; set; }

    private IEnumerable<HeroProfileViewModel>? _heroes;

    protected override async Task HandleOnInitializedAsync()
    {
        await base.HandleOnInitializedAsync();
        
        var heroes = await PlaygroundUiService.GetHeroes();
        _heroes = heroes.OrderBy(h => h.Name);
    }

    private void OnHeroProfileSelected(string heroProfileId)
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/{heroProfileId}");
    }
}

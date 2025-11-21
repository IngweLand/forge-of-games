using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class StatsHubPage : StatsHubPageBase
{
    private TopStatsViewModel? _topStats;

    private IReadOnlyCollection<TreasureHuntLeagueDto>? _treasureHuntLeagues;

    [Inject]
    private ICommonUiService CommonUiService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _treasureHuntLeagues = await LoadWithPersistenceAsync(nameof(_treasureHuntLeagues),
            async () =>
            {
                var result = await CommonUiService.GetTreasureHuntLeaguesAsync();
                return result.Values.Where(x => x.League != TreasureHuntLeague.Undefined).OrderBy(x => x.League)
                    .ToList();
            });

        _topStats = await LoadWithPersistenceAsync(nameof(_topStats),
            async () => await StatsHubUiService.GetTopStatsAsync());
        
        IsInitialized = true;
    }
}

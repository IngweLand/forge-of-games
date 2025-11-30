using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class StatsHubPage : StatsHubPageBase
{
    private readonly CancellationTokenSource _betaCityRankingsCts = new();
    private readonly CancellationTokenSource _mainCityRankingsCts = new();
    private IReadOnlyCollection<PlayerViewModel> _mainWorldCityEventRankings = [];
    private IReadOnlyCollection<PlayerViewModel> _betaWorldCityEventRankings = [];
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

        if (OperatingSystem.IsBrowser())
        {
            _mainWorldCityEventRankings = (await StatsHubUiService.GetEventCityRankingsAsync("un1")).Items
                .Take(FogConstants.DEFAULT_STATS_PAGE_SIZE).ToList();
            _betaWorldCityEventRankings = (await StatsHubUiService.GetEventCityRankingsAsync("zz1")).Items
                .Take(FogConstants.DEFAULT_STATS_PAGE_SIZE).ToList();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _betaCityRankingsCts.Cancel();
            _mainCityRankingsCts.Cancel();
        }

        base.Dispose(disposing);
    }
}

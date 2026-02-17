using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldAlliancesAthStatsPage : WorldStatsPageBase<AllianceViewModel>
{
    private bool _isLoading;
    private ICollection<AllianceViewModel>? _items;
    private TreasureHuntLeague _selectedLeague = TreasureHuntLeague.Overlord;

    private IReadOnlyCollection<TreasureHuntLeagueDto>? _treasureHuntLeagues;

    [Inject]
    private ICommonUiService CommonUiService { get; set; }

    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_TopAllianceAthRankingListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_TopAllianceAthRankingListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override async ValueTask<PaginatedList<AllianceViewModel>> FetchDataAsync(int startIndex, int count,
        string? query = null, CancellationToken ct = default)
    {
        return await StatsHubUiService.GetAlliancesAthRankingsAsync(WorldId, startIndex, count, _selectedLeague, ct);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        IsInitialized = false;

        _treasureHuntLeagues = await LoadWithPersistenceAsync(nameof(_treasureHuntLeagues),
            async () =>
            {
                var result = await CommonUiService.GetTreasureHuntLeaguesAsync();
                return result.Values.Where(x => x.League != TreasureHuntLeague.Undefined).OrderBy(x => x.League)
                    .ToList();
            });

        IsInitialized = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (Enum.TryParse<TreasureHuntLeague>(Q, true, out var l))
        {
            _selectedLeague = l;
        }
        else
        {
            _selectedLeague = TreasureHuntLeague.Overlord;
        }
    }

    private void OnLeagueChanged(TreasureHuntLeague league)
    {
        _selectedLeague = league;
        Search(league.ToString());
    }
}

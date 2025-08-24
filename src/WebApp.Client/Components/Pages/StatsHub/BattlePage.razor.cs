using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class BattlePage : BattlePageBase
{
    private readonly CancellationTokenSource _cts = new();
    private BattleViewModel? _battle;
    private string _battleTitle = string.Empty;

    [Parameter]
    public int BattleId { get; set; }

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    protected override Dictionary<string, object> DefaultAnalyticsParameters { get; } = new()
    {
        {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.BATTLE},
    };

    protected override bool ShouldHidePageLoadingIndicator => false;

    [Inject]
    private ITreasureHuntUiService TreasureHuntUiService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            await JsInteropService.ShowLoadingIndicatorAsync();
            StateHasChanged();
        }

        try
        {
            _battle = await BattleUiService.GetBattleAsync(BattleId, _cts.Token);
        }
        catch (Exception e)
        {
            //ignore
        }

        if (_battle != null)
        {
            _battleTitle = await LoadWithPersistenceAsync(nameof(_battleTitle),
                    async () => await BattleSearchRequestFactory.CreateDefinitionTitleAsync(
                        _battle.Summary.BattleDefinitionId, _battle.Summary.BattleType, _battle.Summary.Difficulty,
                        await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync()))
                ?? string.Empty;
        }

        if (OperatingSystem.IsBrowser())
        {
            await JsInteropService.HideLoadingIndicatorAsync();
        }
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _cts.CancelAsync();

        await base.DisposeAsyncCore();
    }
}

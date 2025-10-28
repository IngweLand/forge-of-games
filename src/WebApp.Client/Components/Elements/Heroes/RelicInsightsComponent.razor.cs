using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class RelicInsightsComponent : ComponentBase, IAsyncDisposable
{
    private CancellationTokenSource? _cts;
    private bool _isLoading = true;
    private IReadOnlyCollection<RelicInsightsViewModel> _relicInsights = [];

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    public ILogger<UnitBattlesComponent> Logger { get; set; }

    [Inject]
    private IRelicUiService RelicUiService { get; set; }

    [Parameter]
    [EditorRequired]
    public required string UnitId { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        await GetRelics();
    }

    private async Task GetRelics()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }

        _cts = new CancellationTokenSource();
        _isLoading = true;
        StateHasChanged();
        try
        {
            _relicInsights = await RelicUiService.GetRelicInsights(UnitId, _cts.Token);
        }
        catch (OperationCanceledException _)
        {
            return;
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error.");
        }

        _isLoading = false;
        StateHasChanged();
    }
}

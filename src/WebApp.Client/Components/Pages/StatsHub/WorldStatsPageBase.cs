using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Models.Fog;
using Microsoft.AspNetCore.Components;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public abstract class WorldStatsPageBase<TItem> : StatsHubPageBase, IAsyncDisposable
{
    private CancellationTokenSource? _cts;
    private Task<PaginatedList<TItem>>? _currentTask;
    private bool _isNavigatingAway;
    protected bool IsLoading => _currentTask is {IsCompleted: false};
    protected IReadOnlyList<TItem> Items { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public string? Q { get; set; }

    protected string? QueryItem { get; private set; }

    protected string Title { get; private set; }

    [Parameter]
    public required string WorldId { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Items = await LoadWithPersistenceAsync(nameof(Items),
            async () =>
            {
                _currentTask = GetDataAsync(0, FogConstants.MAX_LEADERBOARD_PAGE_SIZE);
                var result = await _currentTask;
                return result.Items.ToList();
            }) ?? [];

        IsInitialized = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_isNavigatingAway)
        {
            return;
        }

        await base.OnParametersSetAsync();

        Title = GetTitle();

        QueryItem = Q;

        if (OperatingSystem.IsBrowser())
        {
            _currentTask = GetDataAsync(0, FogConstants.MAX_LEADERBOARD_PAGE_SIZE);
            var result = await _currentTask;
            Items = result.Items.ToList();
            IsInitialized = true;
        }
    }

    protected abstract string GetTitle();

    protected abstract ValueTask<PaginatedList<TItem>> FetchDataAsync(int startIndex, int count, string? query = null,
        CancellationToken ct = default);

    protected async Task<PaginatedList<TItem>> GetDataAsync(int startIndex, int count)
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }

        _cts = new CancellationTokenSource();

        try
        {
            return await FetchDataAsync(startIndex, count, Q, _cts.Token);
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Unexpected error: {ex}");
        }

        return PaginatedList<TItem>.Empty;
    }

    private void NavigateWithQuery(string? queryItem)
    {
        var query = new Dictionary<string, object?>
        {
            {nameof(Q).ToLowerInvariant(), queryItem},
        };

        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(query), false);
    }

    protected void OnItemClicked()
    {
        _isNavigatingAway = true;
    }

    protected void Search(string? name)
    {
        NavigateWithQuery(name);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }
    }
}

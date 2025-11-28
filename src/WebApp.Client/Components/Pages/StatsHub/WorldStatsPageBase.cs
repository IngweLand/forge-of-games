using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public abstract class WorldStatsPageBase<TItem> : StatsHubPageBase
{
    private bool _isNavigatingAway;
    protected PaginatedStatsListComponent<TItem>? _listComponent;

    [Parameter]
    [SupplyParameterFromQuery]
    public string? Q { get; set; }

    protected string? QueryItem { get; private set; }

    protected string Title { get; private set; }

    [Parameter]
    public required string WorldId { get; set; }

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
            await RequestDataRefreshAsync();
            IsInitialized = true;
        }
    }
    
    protected virtual Task RequestDataRefreshAsync()
    {
         _listComponent?.RequestDataRefresh();
         return Task.CompletedTask;
    }

    protected abstract string GetTitle();

    protected abstract ValueTask<PaginatedList<TItem>> FetchDataAsync(ItemsProviderRequest request);

    protected async ValueTask<ItemsProviderResult<TItem>> GetDataAsync(ItemsProviderRequest request)
    {
        try
        {
            var result = await FetchDataAsync(request);
            return new ItemsProviderResult<TItem>(result.Items, result.TotalCount);
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

        return new ItemsProviderResult<TItem>([], 0);
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
}

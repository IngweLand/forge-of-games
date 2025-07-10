using Ingweland.Fog.Models.Fog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public abstract class WorldStatsPageBase<TItem> : StatsHubPageBase
{
    private bool _isNavigatingAway;

    [Parameter]
    [SupplyParameterFromQuery]
    public string? Name { get; set; }

    protected string? NameSearchString { get; private set; }

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

        NameSearchString = Name;

        if (OperatingSystem.IsBrowser())
        {
            IsInitialized = true;
        }
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

    private void NavigateWithQuery(string? nameSearchString)
    {
        var query = new Dictionary<string, object?>
        {
            {nameof(Name), nameSearchString},
        };

        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(query), false);
    }

    protected void OnItemClicked(TItem item)
    {
        _isNavigatingAway = true;
        NavigateToItemPage(item);
    }

    protected abstract void NavigateToItemPage(TItem item);

    protected void Search(string? name)
    {
        Console.Out.WriteLine("Search(string? name)");
        NavigateWithQuery(name);
    }
}

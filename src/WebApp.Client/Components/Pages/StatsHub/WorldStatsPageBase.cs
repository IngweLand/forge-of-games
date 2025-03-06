using Ingweland.Fog.Models.Fog;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public abstract class WorldStatsPageBase<TItem> : StatsHubPageBase
{
    private bool _isNavigatingAway;
    protected PaginatedList<TItem>? Items { get; private set; }
    protected int CalculatedPageNumber { get; private set; } = 1;
    protected CancellationTokenSource? Cts { get; set; }
    protected bool IsLoading { get; private set; } = true;
    protected string? NameSearchString { get; private set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public string? Name { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? PageNumber { get; set; }

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

        IsLoading = true;

        Title = GetTitle();

        var pageNumber = 1;
        if (PageNumber is > 1)
        {
            pageNumber = PageNumber.Value;
        }

        try
        {
            Items = await LoadWithPersistenceAsync(GetPersistenceKey(), async () => await GetData(pageNumber));
        }
        catch (OperationCanceledException _)
        {
            return;
        }

        CalculatedPageNumber = pageNumber;
        NameSearchString = Name;

        IsLoading = false;

        if (OperatingSystem.IsBrowser())
        {
            IsInitialized = true;
        }
    }

    protected abstract string GetTitle();

    protected abstract Task<PaginatedList<TItem>> GetData(int pageNumber);

    private string GetPersistenceKey()
    {
        return $"{nameof(Items)}_{PageNumber}_{Name}";
    }

    private void NavigateWithQuery(int pageNumber)
    {
        Cts?.Cancel();

        var query = new Dictionary<string, object?>()
        {
            {nameof(PageNumber), pageNumber},
            {nameof(Name), NameSearchString}
        };

        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(query), false);
    }

    protected void OnPageChanged(int pageNumber)
    {
        if (pageNumber == PageNumber || (pageNumber == 1 && !PageNumber.HasValue))
        {
            return;
        }

        NavigateWithQuery(pageNumber);
    }

    protected void OnItemClicked(TItem item)
    {
        Cts?.Cancel();
        _isNavigatingAway = true;
        NavigateToItemPage(item);
    }

    protected abstract void NavigateToItemPage(TItem item);

    protected void Search(string? name)
    {
        NameSearchString = name;
        NavigateWithQuery(1);
    }
}
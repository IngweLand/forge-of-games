using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor.Utilities;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;

public abstract partial class StatsHubTopListComponentBase<TItem> : ComponentBase, IAsyncDisposable
{
    protected bool IsLoading { get; set; }
    protected IReadOnlyCollection<TItem>? Items { get; set; }

    protected virtual string ItemsContainerClass => new CssBuilder("items-container").Build();

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }
    
    [Inject]
    protected IStatsHubUiService StatsHubUiService { get; set; }

    protected CancellationTokenSource DataLoadingCts { get; } = new();

    protected override async Task OnParametersSetAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        IsLoading = true;
        StateHasChanged();
        await GetItemsAsync();
        IsLoading = false;
        StateHasChanged();
    }
    
    protected abstract Task GetItemsAsync();

    protected abstract RenderFragment RenderItem(TItem item);

    public async ValueTask DisposeAsync()
    {
        await DataLoadingCts.CancelAsync();
    }
}

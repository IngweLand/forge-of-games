using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Models.Fog;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldEventCityStatsPage : WorldStatsPageBase<PlayerViewModel>
{
    private bool _isLoading;
    private ICollection<PlayerViewModel>? _items;

    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_TopEventCityListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_TopEventCityListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override async ValueTask<PaginatedList<PlayerViewModel>> FetchDataAsync(ItemsProviderRequest request)
    {
        return await StatsHubUiService.GetEventCityRankingsAsync(WorldId, request.CancellationToken);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _items = await LoadWithPersistenceAsync(nameof(_items),
            async () =>
            {
                _isLoading = true;
                var result = await GetDataAsync(new ItemsProviderRequest(0, FogConstants.MAX_EVENT_CITY_RANKINGS,
                    CancellationToken.None));
                _isLoading = false;
                return result.Items.ToList();
            });

        IsInitialized = true;
    }

    protected override Task RequestDataRefreshAsync()
    {
        return Task.CompletedTask;
    }
}

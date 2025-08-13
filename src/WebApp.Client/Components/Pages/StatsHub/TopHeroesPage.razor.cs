using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Helpers;
using Microsoft.AspNetCore.Components;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class TopHeroesPage : StatsHubPageBase, IAsyncDisposable
{
    private IReadOnlyCollection<HeroBasicViewModel> _heroes = [];
    private CancellationTokenSource? _heroesCts;
    private bool _isLoading;

    private TopHeroesSearchFormViewModel _searchFormData;
    private TopHeroesSearchFormRequest? _searchRequest;

    [Inject]
    public IPersistenceService PersistenceService { get; set; }

    [Inject]
    private ITopHeroesUiService TopHeroesUiService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _searchFormData = await TopHeroesUiService.GetTopHeroesSearchFormDataAsync();
        var savedRequest = await PersistenceService.GetTopHeroesRequestAsync();
        TopHeroesSearchFormRequest searchRequest;
        if (savedRequest != null)
        {
            searchRequest = new TopHeroesSearchFormRequest
            {
                Age = _searchFormData.Ages.FirstOrDefault(x => x.Id == savedRequest.Age?.Id),
                LevelRange = _searchFormData.LevelRanges.FirstOrDefault(x => x == savedRequest.LevelRange),
                Mode = _searchFormData.Modes.FirstOrDefault(x => x.Mode == savedRequest.Mode.Mode) ??
                    _searchFormData.Modes.First(),
            };
        }
        else
        {
            searchRequest = new TopHeroesSearchFormRequest
            {
                Mode = _searchFormData.Modes.First(),
            };
        }

        await GetHeroes(searchRequest);
    }

    private async Task GetHeroes(TopHeroesSearchFormRequest request)
    {
        if (_heroesCts != null)
        {
            await _heroesCts.CancelAsync();
        }

        var persistanceTask = PersistenceService.SaveTopHeroesRequestAsync(request);

        _isLoading = true;
        _heroes = [];

        StateHasChanged();

        _heroesCts = new CancellationTokenSource();
        _searchRequest = request;

        try
        {
            _heroes = await TopHeroesUiService.GetTopHeroes(_searchRequest.Mode.Mode, _searchRequest.Age?.Id,
                _searchRequest.LevelRange, _heroesCts.Token);
        }
        catch (OperationCanceledException _)
        {
            return;
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            return;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        _isLoading = false;

        StateHasChanged();

        await persistanceTask;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_heroesCts != null)
        {
            await _heroesCts.CancelAsync();
        }
    }
}

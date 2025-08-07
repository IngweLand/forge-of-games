using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Caching;

public class HohCoreDataViewModelsCache(ITreasureHuntUiService treasureHuntUiService) : IHohCoreDataViewModelsCache
{
    private readonly Lazy<Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>>> _treasureHuntDifficulties =
        new(treasureHuntUiService.GetDifficultiesAsync);

    public Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>> GetBasicTreasureHuntDifficultiesAsync()
    {
        return _treasureHuntDifficulties.Value;
    }
}

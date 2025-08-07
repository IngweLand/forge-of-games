using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Caching.Interfaces;

public interface IHohCoreDataViewModelsCache
{
    Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>> GetBasicTreasureHuntDifficultiesAsync();
}

using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IBattleLogFactories
{
    BattleSelectorViewModel CreateBattleSelectorData(IReadOnlyCollection<ContinentBasicViewModel> campaignContinents,
        IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel> treasureHuntDifficulties,
        IReadOnlyCollection<RegionBasicViewModel> historicBattles,
        IReadOnlyCollection<HeroBasicViewModel> heroes);
}

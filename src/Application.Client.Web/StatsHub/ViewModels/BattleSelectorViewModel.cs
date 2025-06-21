using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class BattleSelectorViewModel
{
    public required IReadOnlyDictionary<BattleType, string> BattleTypes { get; init; }

    public required IReadOnlyCollection<ContinentBasicViewModel> CampaignContinents { get; init; }

    public IReadOnlyCollection<int> CampaignRegionEncounters { get; } = Enumerable.Range(1, 9).ToList();

    public required IReadOnlyDictionary<Difficulty, string> Difficulties { get; init; }

    public required IReadOnlyCollection<HeroBasicViewModel> Heroes { get; init; }
    public required IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel> TreasureHuntDifficulties { get; init; }
    public IReadOnlyCollection<int> TreasureHuntEncounters { get; } = Enumerable.Range(0, 20).ToList();
}

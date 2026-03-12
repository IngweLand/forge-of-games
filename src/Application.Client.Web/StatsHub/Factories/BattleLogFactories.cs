using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class BattleLogFactories(IResourceLocalizationService resourceLocalizationService) : IBattleLogFactories
{
    public BattleSelectorViewModel CreateBattleSelectorData(
        IReadOnlyCollection<ContinentBasicViewModel> campaignContinents,
        IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel> treasureHuntDifficulties,
        IReadOnlyCollection<RegionBasicViewModel> historicBattles,
        IReadOnlyCollection<RegionBasicViewModel> teslaStormRegions,
        IReadOnlyCollection<HeroBasicViewModel> heroes, IReadOnlyCollection<BattleEventBasicViewModel> battleEvents)
    {
        return new BattleSelectorViewModel
        {
            BattleTypes = new Dictionary<BattleType, string>
            {
                {
                    BattleType.Campaign, resourceLocalizationService.Localize(BattleType.Campaign)
                },
                {
                    BattleType.TreasureHunt, resourceLocalizationService.Localize(BattleType.TreasureHunt)
                },
                {
                    BattleType.Pvp, resourceLocalizationService.Localize(BattleType.Pvp)
                },
                {
                    BattleType.HistoricBattle, resourceLocalizationService.Localize(BattleType.HistoricBattle)
                },
                {
                    BattleType.TeslaStorm, resourceLocalizationService.Localize(BattleType.TeslaStorm)
                },
                {
                    BattleType.BattleEvent, resourceLocalizationService.Localize(BattleType.BattleEvent)
                },
            },
            CampaignContinents = campaignContinents,
            Difficulties = new Dictionary<Difficulty, string>
            {
                {
                    Difficulty.Normal, resourceLocalizationService.Localize(Difficulty.Normal)
                },
                {
                    Difficulty.Hard, resourceLocalizationService.Localize(Difficulty.Hard)
                },
            },
            TreasureHuntDifficulties = treasureHuntDifficulties,
            HistoricBattleRegions = historicBattles,
            TeslaStormRegions = teslaStormRegions,
            Heroes = heroes,
            BattleEvents = battleEvents,
        };
    }
}

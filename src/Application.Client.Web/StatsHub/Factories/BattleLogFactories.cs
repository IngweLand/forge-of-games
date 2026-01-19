using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class BattleLogFactories(
    IStringLocalizer<FogResource> localizer) : IBattleLogFactories
{
    public BattleSelectorViewModel CreateBattleSelectorData(
        IReadOnlyCollection<ContinentBasicViewModel> campaignContinents,
        IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel> treasureHuntDifficulties,
        IReadOnlyCollection<RegionBasicViewModel> historicBattles,
        IReadOnlyCollection<RegionBasicViewModel> teslaStormRegions,
        IReadOnlyCollection<HeroBasicViewModel> heroes)
    {
        return new BattleSelectorViewModel
        {
            BattleTypes = new Dictionary<BattleType, string>
            {
                {BattleType.Campaign, localizer[FogResource.BattleType_Campaign]},
                {BattleType.TreasureHunt, localizer[FogResource.BattleType_TreasureHunt]},
                {BattleType.Pvp, localizer[FogResource.BattleType_PvP]},
                {BattleType.HistoricBattle, localizer[FogResource.BattleType_HistoricBattle]},
                {BattleType.TeslaStorm, localizer[FogResource.BattleType_TeslaStorm]},
                // {BattleType.AncientEgypt, localizer[FogResource.BattleType_AncientEgypt]},
            },
            CampaignContinents = campaignContinents,
            Difficulties = new Dictionary<Difficulty, string>
            {
                {Difficulty.Normal, localizer[FogResource.Battle_Difficulty_Normal]},
                {Difficulty.Hard, localizer[FogResource.Battle_Difficulty_Hard]},
            },
            TreasureHuntDifficulties = treasureHuntDifficulties,
            HistoricBattleRegions = historicBattles,
            TeslaStormRegions = teslaStormRegions,
            Heroes = heroes,
        };
    }
}

using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleViewModelFactory
{
    Task<IReadOnlyCollection<BattleSummaryViewModel>> CreateBattleSummaryViewModels(
        IReadOnlyCollection<BattleSummaryDto> battles, BattleType battleType);

    PvpBattleViewModel CreatePvpBattle(PlayerViewModel player, PvpBattleDto pvpBattleDto,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        IReadOnlyDictionary<string, RelicDto> relics);

    IReadOnlyCollection<UnitBattleViewModel> CreateUnitBattleViewModels(
        IReadOnlyCollection<UnitBattleDto> unitBattles);

    IReadOnlyCollection<UnitBattleTypeViewModel> CreateUnitBattleTypes(IEnumerable<BattleType> unitBattleTypes);
    Task<BattleViewModel> CreateBattleViewModel(BattleDto battle);
}

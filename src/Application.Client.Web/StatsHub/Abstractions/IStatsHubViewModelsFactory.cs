using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IStatsHubViewModelsFactory
{
    PlayerProfileViewModel CreatePlayerProfile(PlayerProfile playerProfile,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks);

    PaginatedList<PlayerViewModel> CreatePlayers(PaginatedList<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages);

    TopStatsViewModel CreateTopStats(IReadOnlyCollection<PlayerDto> mainPlayers,
        IReadOnlyCollection<PlayerDto> betaPlayers, IReadOnlyCollection<AllianceDto> mainAlliances,
        IReadOnlyCollection<AllianceDto> betaAlliances, IReadOnlyDictionary<string, AgeDto> ages);

    AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,
        IReadOnlyDictionary<string, AgeDto> ages);

    PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> players);

    BattleSummaryViewModel CreateBattleSummaryViewModel(BattleSummaryDto summaryDto,
        IReadOnlyDictionary<string, HeroDto> heroes,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks);

    IReadOnlyCollection<UnitBattleViewModel> CreateUnitBattleViewModels(
        IReadOnlyCollection<UnitBattleDto> unitBattles);

    IReadOnlyCollection<UnitBattleTypeViewModel> CreateUnitBattleTypes(IEnumerable<BattleType> unitBattleTypes);

    PvpBattleViewModel CreatePvpBattle(PlayerViewModel player, PvpBattleDto pvpBattleDto,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks);
}

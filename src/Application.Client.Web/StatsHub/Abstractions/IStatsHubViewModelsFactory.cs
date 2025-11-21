using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IStatsHubViewModelsFactory
{
    PlayerProfileViewModel CreatePlayerProfile(PlayerProfileDto playerProfile,
        IReadOnlyCollection<HeroDto> heroes, IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto> barracks,
        TreasureHuntDifficultyBasicViewModel? treasureHuntDifficulty, int treasureHuntMaxPoints,
        IReadOnlyDictionary<string, RelicDto> relics);

    PaginatedList<PlayerViewModel> CreatePlayers(PaginatedList<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages);

    TopStatsViewModel CreateTopStats(IReadOnlyCollection<PlayerDto> mainPlayers,
        IReadOnlyCollection<PlayerDto> betaPlayers,
        IReadOnlyCollection<AllianceDto> mainAlliances, IReadOnlyCollection<AllianceDto> betaAlliances,
        IReadOnlyCollection<string> topHeroes,
        IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyCollection<HeroBasicViewModel> heroes);

    AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,
        IReadOnlyDictionary<string, AgeDto> ages);

    PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> players);
}

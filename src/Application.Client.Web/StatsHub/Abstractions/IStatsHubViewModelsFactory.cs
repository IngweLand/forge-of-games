using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
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

    IReadOnlyCollection<PlayerViewModel> CreatePlayers(IReadOnlyCollection<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages);

    AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,
        IReadOnlyDictionary<string, AgeDto> ages,
        IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel> treasureHuntDifficulties,
        IReadOnlyDictionary<int, int> treasureHuntMaxPointsMap);

    PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> alliances);
    IReadOnlyCollection<AllianceViewModel> CreateAlliances(IReadOnlyCollection<AllianceDto> alliances);
}

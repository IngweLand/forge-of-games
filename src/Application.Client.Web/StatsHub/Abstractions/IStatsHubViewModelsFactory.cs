using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IStatsHubViewModelsFactory
{
    PlayerWithRankingsViewModel CreatePlayer(PlayerWithRankings playerWithRankings, IReadOnlyDictionary<string, AgeDto> ages);

    PaginatedList<PlayerViewModel> CreatePlayers(PaginatedList<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages);

    TopStatsViewModel CreateTopStats(IReadOnlyCollection<PlayerDto> mainPlayers,
        IReadOnlyCollection<PlayerDto> betaPlayers, IReadOnlyCollection<AllianceDto> mainAlliances,
        IReadOnlyCollection<AllianceDto> betaAlliances, IReadOnlyDictionary<string, AgeDto> ages);

    AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,
        IReadOnlyDictionary<string, AgeDto> ages);

    PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> players);
}
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class StatsHubViewModelsFactory(IMapper mapper) : IStatsHubViewModelsFactory
{
    public PaginatedList<PlayerViewModel> CreatePlayers(PaginatedList<PlayerDto> players,
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        return mapper.Map<PaginatedList<PlayerViewModel>>(players,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }

    public AllianceWithRankingsViewModel CreateAlliance(AllianceWithRankings alliance,IReadOnlyDictionary<string, AgeDto> ages)
    {
        return mapper.Map<AllianceWithRankingsViewModel>(alliance,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }

    public PaginatedList<AllianceViewModel> CreateAlliances(PaginatedList<AllianceDto> players)
    {
        return mapper.Map<PaginatedList<AllianceViewModel>>(players);
    }

    public TopStatsViewModel CreateTopStats(IReadOnlyCollection<PlayerDto> mainPlayers,
        IReadOnlyCollection<PlayerDto> betaPlayers,
        IReadOnlyCollection<AllianceDto> mainAlliances, IReadOnlyCollection<AllianceDto> betaAlliances,
        IReadOnlyDictionary<string, AgeDto> ages)
    {
        return new TopStatsViewModel()
        {
            MainWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(mainPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            BetaWorldPlayers = mapper.Map<IReadOnlyCollection<PlayerViewModel>>(betaPlayers,
                opt => { opt.Items[ResolutionContextKeys.AGES] = ages; }),
            MainWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(mainAlliances),
            BetaWorldAlliances = mapper.Map<IReadOnlyCollection<AllianceViewModel>>(betaAlliances)
        };
    }

    public PlayerWithRankingsViewModel CreatePlayer(PlayerWithRankings player, IReadOnlyDictionary<string, AgeDto> ages)
    {
        return mapper.Map<PlayerWithRankingsViewModel>(player,
            opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }
}
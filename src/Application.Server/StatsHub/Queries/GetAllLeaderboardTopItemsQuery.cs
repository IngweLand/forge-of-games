using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllLeaderboardTopItemsQuery : IRequest<LeaderboardTopItemsDto>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class
    GetAllLeaderboardTopItemsQueryHandler(IStatsHubService statsHubService)
    : IRequestHandler<GetAllLeaderboardTopItemsQuery, LeaderboardTopItemsDto>
{
    public async Task<LeaderboardTopItemsDto> Handle(GetAllLeaderboardTopItemsQuery request,
        CancellationToken cancellationToken)
    {
        var mainPlayers =
            await statsHubService.GetPlayersAsync("un1", ct: cancellationToken);
        var betaPlayers =
            await statsHubService.GetPlayersAsync("zz1", ct: cancellationToken);
        var mainAlliances =
            await statsHubService.GetAlliancesAsync("un1", ct: cancellationToken);
        var betaAlliances =
            await statsHubService.GetAlliancesAsync("zz1", ct: cancellationToken);
        var topHeroes = await statsHubService.GetTopHeroesAsync(HeroInsightsMode.MostPopular, ct: cancellationToken);
        return new LeaderboardTopItemsDto
        {
            MainWorldPlayers = mainPlayers,
            BetaWorldPlayers = betaPlayers,
            MainWorldAlliances = mainAlliances,
            BetaWorldAlliances = betaAlliances,
            TopHeroes = topHeroes,
        };
    }
}

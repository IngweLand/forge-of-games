using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Shared.Utils;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllLeaderboardTopItemsQuery : IRequest<LeaderboardTopItemsDto>, ICacheableRequest
{
    public string CacheKey => "LeaderboardTopItems";
    public TimeSpan? Duration => null;
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class
    GetAllLeaderboardTopItemsQueryHandler(IStatsHubService statsHubService)
    : IRequestHandler<GetAllLeaderboardTopItemsQuery, LeaderboardTopItemsDto>
{
    public async Task<LeaderboardTopItemsDto> Handle(GetAllLeaderboardTopItemsQuery request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
        try
        {
            var mainPlayers = await statsHubService.GetPlayersAsync("un1", ct: cancellationToken);
            var betaPlayers = await statsHubService.GetPlayersAsync("zz1", ct: cancellationToken);
            var mainAlliances = await statsHubService.GetAlliancesAsync("un1", ct: cancellationToken);
            var betaAlliances = await statsHubService.GetAlliancesAsync("zz1", ct: cancellationToken);
            return new LeaderboardTopItemsDto
            {
                MainWorldPlayers = mainPlayers,
                BetaWorldPlayers = betaPlayers,
                MainWorldAlliances = mainAlliances,
                BetaWorldAlliances = betaAlliances,
            };
        }
        catch
        {
            return LeaderboardTopItemsDto.Blank;
        }
    }
}

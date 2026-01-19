using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerCityStrategiesQuery(int PlayerId)
    : IRequest<Result<IReadOnlyCollection<PlayerCityStrategyInfoDto>>>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerCityStrategiesQueryHandler(
    IFogDbContext context,
    IPlayerCityStrategyInfoDtoFactory dtoFactory)
    : IRequestHandler<GetPlayerCityStrategiesQuery, Result<IReadOnlyCollection<PlayerCityStrategyInfoDto>>>
{
    public async Task<Result<IReadOnlyCollection<PlayerCityStrategyInfoDto>>> Handle(
        GetPlayerCityStrategiesQuery request, CancellationToken cancellationToken)
    {
        var playerResult = await Result.Try(() =>
            context.Players.AsNoTracking()
                .Include(x => x.EventCityStrategies)
                .ThenInclude(x => x.InGameEvent)
                .FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken));

        if (playerResult.IsFailed)
        {
            return playerResult.ToResult<IReadOnlyCollection<PlayerCityStrategyInfoDto>>();
        }

        if (playerResult.Value == null)
        {
            return Result.Fail<IReadOnlyCollection<PlayerCityStrategyInfoDto>>(
                new FogPlayerNotFoundError(request.PlayerId));
        }

        return playerResult.Value.EventCityStrategies
            .Select(dtoFactory.Create)
            .ToList();
    }
}

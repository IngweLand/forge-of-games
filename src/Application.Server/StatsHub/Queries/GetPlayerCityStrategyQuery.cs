using FluentResults;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerCityStrategyQuery(int StrategyId)
    : IRequest<Result<CityStrategy>>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(24);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerCityStrategyQueryHandler(IFogDbContext context, IProtobufSerializer protobufSerializer)
    : IRequestHandler<GetPlayerCityStrategyQuery, Result<CityStrategy>>
{
    public async Task<Result<CityStrategy>> Handle(GetPlayerCityStrategyQuery request,
        CancellationToken cancellationToken)
    {
        var strategyResult = await Result.Try(() => context.EventCityStrategies.AsNoTracking()
            .Include(x => x.Data)
            .FirstOrDefaultAsync(x => x.Id == request.StrategyId, cancellationToken));
        if (strategyResult.IsFailed)
        {
            strategyResult.LogIfFailed<GetPlayerCityStrategyQueryHandler>();
            return strategyResult.ToResult<CityStrategy>();
        }

        return Result.Try(() => protobufSerializer.DeserializeFromBytes<CityStrategy>(strategyResult.Value!.Data.Data));
    }
}

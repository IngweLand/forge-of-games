using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetSharedCityStrategiesQuery : IRequest<Result<IReadOnlyCollection<CommunityCityStrategyDto>>>,
    ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(1);
    public DateTimeOffset? Expiration { get; }
}

public class GetSharedCityStrategiesQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetSharedCityStrategiesQuery, Result<IReadOnlyCollection<CommunityCityStrategyDto>>>
{
    public Task<Result<IReadOnlyCollection<CommunityCityStrategyDto>>> Handle(
        GetSharedCityStrategiesQuery request, CancellationToken cancellationToken)
    {
        return Result.Try(() => context.CommunityCityStrategies
                .ProjectTo<CommunityCityStrategyDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken))
            .Map(IReadOnlyCollection<CommunityCityStrategyDto> (x) => x);
    }
}

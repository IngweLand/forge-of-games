using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetCommunityCityGuidesQuery : IRequest<Result<IReadOnlyCollection<CommunityCityGuideInfoDto>>>,
    ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(1);
    public DateTimeOffset? Expiration { get; }
}

public class GetCommunityCityGuidesQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetCommunityCityGuidesQuery, Result<IReadOnlyCollection<CommunityCityGuideInfoDto>>>
{
    public Task<Result<IReadOnlyCollection<CommunityCityGuideInfoDto>>> Handle(
        GetCommunityCityGuidesQuery request, CancellationToken cancellationToken)
    {
        return Result.Try(() => context.CommunityCityGuides
                .ProjectTo<CommunityCityGuideInfoDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken))
            .Map(IReadOnlyCollection<CommunityCityGuideInfoDto> (x) => x);
    }
}

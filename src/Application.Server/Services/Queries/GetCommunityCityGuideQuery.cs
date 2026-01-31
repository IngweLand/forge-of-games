using AutoMapper;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetCommunityCityGuideQuery(int Id) : IRequest<Result<CommunityCityGuideDto>>,
    ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(1);
    public DateTimeOffset? Expiration { get; }
}

public class GetCommunityCityGuideQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetCommunityCityGuideQuery, Result<CommunityCityGuideDto>>
{
    public async Task<Result<CommunityCityGuideDto>> Handle(
        GetCommunityCityGuideQuery request, CancellationToken cancellationToken)
    {
        return await Result.Try(() => context.CommunityCityGuides.FindAsync(request.Id, cancellationToken),
                ex => new Error("Error getting community city guide.").CausedBy(ex))
            .Bind(x => x != null
                ? mapper.Map<CommunityCityGuideDto>(x)
                : Result.Fail<CommunityCityGuideDto>(new EntityNotFoundError(nameof(CommunityCityGuideEntity),
                    request.Id)));
    }
}

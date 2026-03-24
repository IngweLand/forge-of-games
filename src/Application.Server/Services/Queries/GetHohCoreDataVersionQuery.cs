using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetHohCoreDataVersionQuery : IRequest<VersionDto>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromMinutes(2);
    public DateTimeOffset? Expiration { get; }
}

public class GetHohCoreDataVersionQueryHandler(IOptionsSnapshot<ResourceSettings> resourceSettings)
    : IRequestHandler<GetHohCoreDataVersionQuery, VersionDto>
{
    public Task<VersionDto> Handle(GetHohCoreDataVersionQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new VersionDto(resourceSettings.Value.HohCoreDataVersion));
    }
}

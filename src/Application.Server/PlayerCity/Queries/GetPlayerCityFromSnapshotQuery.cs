using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record GetPlayerCityFromSnapshotQuery(int SnapshotId) : IRequest<HohCity?>, ICacheableRequest
{
    public string CacheKey => $"PlayerCityFromSnapshot_{SnapshotId}";
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class GetPlayerCityFromSnapshotQueryHandler(
    IFogDbContext context,
    IHohCityCreationService cityCreationService)
    : IRequestHandler<GetPlayerCityFromSnapshotQuery, HohCity?>
{
    public async Task<HohCity?> Handle(GetPlayerCityFromSnapshotQuery request, CancellationToken cancellationToken)
    {
        var snapshot = await context.PlayerCitySnapshots.Include(x => x.Player)
            .FirstOrDefaultAsync(p => p.Id == request.SnapshotId, cancellationToken);
        if (snapshot == null)
        {
            return null;
        }

        return await cityCreationService.Create(snapshot, snapshot.Player.Name);
    }
}

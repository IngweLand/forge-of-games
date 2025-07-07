using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.PlayerCity.Queries;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CityPlannerService(ISender sender) : ICityPlannerService
{
    public Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> GetInspirationsAsync(
        CityInspirationsSearchRequest request, CancellationToken ct = default)
    {
        return sender.Send(new CityInspirationsSearchQuery(request), ct);
    }

    public Task<HohCity?> GetPlayerCitySnapshotAsync(int snapshotId, CancellationToken ct = default)
    {
        return sender.Send(new GetPlayerCityFromSnapshotQuery(snapshotId), ct);
    }
}

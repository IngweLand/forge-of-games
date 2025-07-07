using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICityPlannerService
{
    [Post(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOTS_SEARCH)]
    public Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> GetInspirationsAsync(
        [Body] CityInspirationsSearchRequest request, CancellationToken ct = default);

    [Get(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOT_TEMPLATE_REFIT)]
    Task<HohCity?> GetPlayerCitySnapshotAsync(int snapshotId, CancellationToken ct = default);
}

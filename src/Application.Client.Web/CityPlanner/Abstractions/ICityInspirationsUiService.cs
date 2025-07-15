using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityInspirationsUiService
{
    Task<CityInspirationsSearchFormViewModel> GetSearchFormDataAsync();

    Task<IReadOnlyCollection<PlayerCitySnapshotBasicViewModel>> GetInspirationsAsync(CityInspirationsSearchRequest request,
        CancellationToken ct = default);

    Task<HohCity?> GetPlayerCitySnapshotAsync(int snapshotId, CancellationToken ct = default);

    Task<int> CalculateTotalAreaAsync(CityId cityId, int expansionCount);
}

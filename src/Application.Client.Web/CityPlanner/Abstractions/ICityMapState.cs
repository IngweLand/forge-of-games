using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapState
{
    IReadOnlyList<CityMapEntity> CityMapEntities { get; }
    IReadOnlyList<CityMapEntity> HappinessConsumers { get; }
    IReadOnlyList<CityMapEntity> HappinessProviders { get; }
    void AddRange(IEnumerable<CityMapEntity> cityMapEntities);
    void Reset();
    void Add(CityMapEntity cityMapEntity);
    void Remove(CityMapEntity cityMapEntity);
}

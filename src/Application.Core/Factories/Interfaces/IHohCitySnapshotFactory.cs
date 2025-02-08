using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IHohCitySnapshotFactory
{
    HohCitySnapshot Create(IEnumerable<HohCityMapEntity> cityMapEntities);
    HohCitySnapshot Create(IEnumerable<HohCityMapEntity> cityMapEntities, string? name);
}

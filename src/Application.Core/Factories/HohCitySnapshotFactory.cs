using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.Factories;

public class HohCitySnapshotFactory() : IHohCitySnapshotFactory
{
    public HohCitySnapshot Create(IEnumerable<HohCityMapEntity> cityMapEntities)
    {
        return Create(cityMapEntities, null);
    }

    public HohCitySnapshot Create(IEnumerable<HohCityMapEntity> cityMapEntities, string? name)
    {
        return new HohCitySnapshot()
        {
            Id = Guid.NewGuid().ToString(),
            CreatedDateUtc = DateTime.UtcNow,
            Name = name,
            Entities = cityMapEntities.Select(src => src.Clone()).ToList(),
        };
    }
}

using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

public interface ICityMapEntityStatsFactory
{
    IReadOnlyCollection<ICityMapEntityStats> Create(BuildingDto building);
}

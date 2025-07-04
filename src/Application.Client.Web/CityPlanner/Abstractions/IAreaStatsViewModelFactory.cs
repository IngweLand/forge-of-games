using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IAreaStatsViewModelFactory
{
    AreaStatsViewModel Create(CityStats stats, IEnumerable<BuildingDto> buildings);
}

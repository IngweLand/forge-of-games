using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface ICityStatsCalculator
{
    Task<CityStats> Calculate(HohCity city);
}

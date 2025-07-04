using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public static class StatsExtensions
{
    public static bool HasStat<TStat>(this CityMapEntity entity)
        where TStat : ICityMapEntityStats
    {
        return entity.Stats.OfType<TStat>().Any();
    }
    
    public static TStat? FirstOrDefaultStat<TStat>(this CityMapEntity entity)
        where TStat : ICityMapEntityStats
    {
        return entity.Stats.OfType<TStat>().FirstOrDefault();
    }
    
    public static TStat FirstStat<TStat>(this CityMapEntity entity)
        where TStat : ICityMapEntityStats
    {
        return entity.Stats.OfType<TStat>().First();
    }
}

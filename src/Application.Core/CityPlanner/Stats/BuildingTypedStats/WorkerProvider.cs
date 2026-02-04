using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

public class WorkerProvider : ICityMapEntityStats
{
    public required IReadOnlyDictionary<WorkerType, int> Workers { get; init; }
}

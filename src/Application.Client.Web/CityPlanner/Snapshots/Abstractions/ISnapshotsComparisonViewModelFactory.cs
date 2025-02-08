using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots.Abstractions;

public interface ISnapshotsComparisonViewModelFactory
{
    SnapshotsComparisonViewModel Create(IReadOnlyDictionary<HohCitySnapshot, CityStats> stats);
}

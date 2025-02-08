namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;

public class SnapshotsComparisonViewModel
{
    public required IReadOnlyCollection<string> SnapshotNames { get; init; }
    public required IReadOnlyCollection<SnapshotsComparisonRow> Production { get; init; }
}

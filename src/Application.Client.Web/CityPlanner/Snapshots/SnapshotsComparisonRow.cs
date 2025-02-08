namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;

public class SnapshotsComparisonRow
{
    public required string Header { get; init; }
    public required IReadOnlyCollection<SnapshotsComparisonCellValue> Cells { get; init; }
}

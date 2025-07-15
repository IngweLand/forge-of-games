using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class AreaStatsViewModel
{
    public required IReadOnlyCollection<(string GroupName, string Area)> AreasByGroup { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> AreasByType { get; init; }
    public required string TotalArea { get; init; }
}

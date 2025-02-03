using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class HappinessStatsViewModel
{
    public required IconLabelItemViewModel ExcessHappiness { get; init; }
    public required IconLabelItemViewModel TotalAvailableHappiness { get; init; }
    public required IconLabelItemViewModel TotalHappinessConsumption { get; init; }
    public required IconLabelItemViewModel UnmetHappinessNeed { get; init; }
    public required IconLabelItemViewModel UsageRatio { get; init; }
}

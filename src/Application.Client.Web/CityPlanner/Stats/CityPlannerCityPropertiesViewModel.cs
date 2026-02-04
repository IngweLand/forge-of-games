using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityPlannerCityPropertiesViewModel
{
    public required AgeViewModel Age { get; set; }
    public required AreaStatsViewModel Areas { get; set; }
    public required CityId CityId { get; init; }
    public required HappinessStatsViewModel Happiness { get; set; }
    public required string Name { get; set; }
    public required ProductionStatsViewModel Production { get; set; }
    public IReadOnlyCollection<IconLabelItemViewModel>? WonderBonus { get; init; }
    public IReadOnlyCollection<IconLabelItemViewModel>? WonderCost { get; init; }
    public int WonderLevel { get; set; }
    public string? WonderNextLevelRangeLabel { get; set; }

    public required IReadOnlyCollection<IconLabelItemViewModel> Workforce { get; set; }
}

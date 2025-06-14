using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityPlannerCityPropertiesViewModel
{
    public required CityId CityId { get; init; }
    public required AgeViewModel Age { get; set; }
    public required string Name { get; set; }

    public required IconLabelItemViewModel Workforce { get; set; }
    public required HappinessStatsViewModel Happiness { get; set; }
    public required ProductionStatsViewModel Production { get; set; }
    public required AreaStatsViewModel Areas { get; set; }
    public string? WonderName { get; set; }
    public int WonderLevel { get; set; }
    
}

using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;

public class HappinessConsumer:ICityMapEntityStats
{
    public int ConsumedHappiness { get; set; }
    public required BuildingBuffDetails BuffDetails { get; init; }
}

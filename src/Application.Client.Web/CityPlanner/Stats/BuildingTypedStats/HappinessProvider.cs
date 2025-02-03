using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;

public class HappinessProvider:ICityMapEntityStats
{
    public required CultureComponent CultureComponent { get; init; }
    public int Range { get; set; }
    public int Value { get; set; }
    
    public void Update(string ageId, int level)
    {
        Value = CultureComponent.GetValue(ageId, level);
        Range = CultureComponent.GetRange(level);
    }
}

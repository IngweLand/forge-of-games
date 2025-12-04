using System.Text.Json.Serialization;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class StatsTimedIntValue
{
    public required DateTime Date { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required int Value { get; init; }
}

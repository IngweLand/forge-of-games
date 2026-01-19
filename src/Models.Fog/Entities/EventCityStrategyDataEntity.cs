namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCityStrategyDataEntity
{
    public required byte[] Data { get; set; }
    public EventCityStrategy EventCityStrategy { get; set; }
    public int EventCityStrategyId { get; set; }
    public int Id { get; set; }
}

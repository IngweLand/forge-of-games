namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

public class CityStrategyTimelineItemCreateRequest
{
    public string? ExistingCityId { get; set; }
    public required string ItemId { get; init; }
    public required CityStrategyNewTimelineItemType Type { get; init; }
}

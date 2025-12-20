using Ingweland.Fog.Models.Fog.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
[ProtoInclude(100, typeof(CityStrategyTimelineDescriptionItem))]
[ProtoInclude(101, typeof(CityStrategyTimelineResearchItem))]
[ProtoInclude(102, typeof(CityStrategyTimelineLayoutItem))]
public abstract class CityStrategyTimelineItemBase
{
    [ProtoMember(1)]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [ProtoIgnore]
    public bool Selected { get; set; }

    [ProtoMember(2)]
    public required string Title { get; set; }

    [ProtoIgnore]
    public abstract CityStrategyTimelineItemType Type { get; }
}

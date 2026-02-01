using Ingweland.Fog.Models.Fog.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
[ProtoInclude(100, typeof(CityStrategyDescriptionTimelineItem))]
[ProtoInclude(101, typeof(CityStrategyResearchTimelineItem))]
[ProtoInclude(102, typeof(CityStrategyLayoutTimelineItem))]
[ProtoInclude(103, typeof(CityStrategyIntroTimelineItem))]
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

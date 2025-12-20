using Ingweland.Fog.Models.Fog.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class CityStrategyTimelineDescriptionItem : CityStrategyTimelineItemBase
{
    [ProtoMember(1)]
    public required string Description { get; set; }
    public override CityStrategyTimelineItemType Type => CityStrategyTimelineItemType.Description;
}

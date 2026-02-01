using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class CityStrategyIntroTimelineItem : CityStrategyTimelineItemBase
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }

    [ProtoMember(2)]
    public string Description { get; set; } = string.Empty;

    public override CityStrategyTimelineItemType Type => CityStrategyTimelineItemType.Intro;

    [ProtoMember(3)]
    public WonderId WonderId { get; init; } = WonderId.Undefined;
}

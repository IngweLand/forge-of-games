using Ingweland.Fog.Models.Fog.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class CityStrategyResearchTimelineItem : CityStrategyTimelineItemBase
{
    [ProtoIgnore]
    public IReadOnlySet<string> OpenedTechnologies { get; set; } = new HashSet<string>();

    [ProtoMember(1)]
    public ISet<string> Technologies { get; set; } = new HashSet<string>();

    public override CityStrategyTimelineItemType Type => CityStrategyTimelineItemType.Research;
}

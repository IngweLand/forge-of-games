using Ingweland.Fog.Models.Fog.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class CityStrategyLayoutTimelineItem : CityStrategyTimelineItemBase
{
    [ProtoMember(1)]
    public required string AgeId { get; set; }

    [ProtoMember(2)]
    public IReadOnlyCollection<HohCityMapEntity> Entities { get; set; } = new List<HohCityMapEntity>();

    public override CityStrategyTimelineItemType Type => CityStrategyTimelineItemType.Layout;

    [ProtoMember(3)]
    public HashSet<string> UnlockedExpansions { get; set; } = new();

    [ProtoMember(4)]
    public int WonderLevel { get; set; }
}

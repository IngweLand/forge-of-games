using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class CityStrategy
{
    [ProtoMember(1)]
    public required string AgeId { get; set; }

    [ProtoMember(2)]
    public int CityPlannerVersion { get; set; }

    [ProtoMember(3)]
    public required string Id { get; set; }

    [ProtoMember(4)]
    public CityId InGameCityId { get; set; }

    [ProtoMember(5)]
    public required string Name { get; set; }

    [ProtoMember(6)]
    public IList<CityStrategyTimelineItemBase> Timeline { get; set; } = new List<CityStrategyTimelineItemBase>();

    [ProtoMember(7)]
    public DateTime UpdatedAt { get; set; }

    [ProtoMember(8)]
    public WonderId WonderId { get; set; }
}

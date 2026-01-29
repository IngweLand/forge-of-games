using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class HohCity
{
    [ProtoMember(1)]
    public required string AgeId { get; set; }

    [ProtoMember(2)]
    public int CityPlannerVersion { get; set; }

    [ProtoMember(3)]
    public IReadOnlyCollection<HohCityMapEntity> Entities { get; set; } = new List<HohCityMapEntity>();

    [ProtoMember(4)]
    public required string Id { get; set; }

    [ProtoMember(5)]
    public CityId InGameCityId { get; set; }

    [ProtoMember(6)]
    public IReadOnlyCollection<HohCityMapEntity> InventoryBuildings { get; set; } = new List<HohCityMapEntity>();

    [ProtoMember(7)]
    public required string Name { get; set; }

    [ProtoMember(8)]
    public int PremiumExpansionCount { get; set; }

    [ProtoMember(9)]
    public IReadOnlyCollection<HohCitySnapshot> Snapshots { get; init; } = new List<HohCitySnapshot>();

    [ProtoMember(10)]
    public HashSet<string> UnlockedExpansions { get; set; } = [];

    [ProtoMember(11)]
    public DateTime UpdatedAt { get; set; }

    [ProtoMember(12)]
    public WonderId WonderId { get; set; }

    [ProtoMember(13)]
    public int WonderLevel { get; set; }
}

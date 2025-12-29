using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class EquipmentProfile : VersionedModel
{
    [ProtoMember(1)]
    public required BarracksProfile BarracksProfile { get; init; }

    [ProtoMember(2)]
    public IList<HeroEquipmentConfiguration> Configurations { get; set; } = [];

    [ProtoMember(3)]
    public IReadOnlyCollection<EquipmentItem> Equipment { get; init; } = new List<EquipmentItem>();

    [ProtoMember(4)]
    public int EquipmentConfiguratorVersion { get; set; }

    [ProtoMember(5)]
    public IReadOnlyCollection<HeroProfileIdentifier> Heroes { get; init; } = new List<HeroProfileIdentifier>();

    [ProtoMember(6)]
    public required string Id { get; set; }

    [ProtoMember(7)]
    public required string Name { get; set; }

    [ProtoMember(8)]
    public IReadOnlyCollection<RelicItem> Relics { get; init; } = new List<RelicItem>();

    [ProtoMember(9)]
    public required DateTime UpdatedAt { get; set; }
}

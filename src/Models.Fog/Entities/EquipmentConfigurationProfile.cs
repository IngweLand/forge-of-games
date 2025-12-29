using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Models.Fog.Entities;

public class EquipmentConfigurationProfile : VersionedModel
{
    public required BarracksProfile BarracksProfile { get; init; }
    public IList<HeroEquipmentConfiguration> Configurations { get; init; } = new List<HeroEquipmentConfiguration>();
    public IReadOnlyCollection<EquipmentItem> Equipment { get; init; } = new List<EquipmentItem>();

    public int EquipmentConfiguratorVersion { get; set; }
    public IReadOnlyCollection<HeroProfileIdentifier> Heroes { get; init; } = new List<HeroProfileIdentifier>();
    public required string Id { get; set; }
    public required string Name { get; set; }
    public IReadOnlyCollection<RelicItem> Relics { get; init; } = new List<RelicItem>();
}

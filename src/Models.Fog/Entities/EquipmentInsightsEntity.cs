using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class EquipmentInsightsEntity
{
    public required ICollection<EquipmentSet> EquipmentSets { get; set; }
    public required EquipmentSlotType EquipmentSlotType { get; set; }
    public required int FromLevel { get; set; }
    public int Id { get; set; }
    public required ICollection<StatAttribute> MainAttributes { get; set; }
    public ICollection<StatAttribute> SubAttributesLevel12 { get; set; } = new List<StatAttribute>();
    public ICollection<StatAttribute> SubAttributesLevel16 { get; set; } = new List<StatAttribute>();
    public ICollection<StatAttribute> SubAttributesLevel4 { get; set; } = new List<StatAttribute>();
    public ICollection<StatAttribute> SubAttributesLevel8 { get; set; } = new List<StatAttribute>();
    public required int ToLevel { get; set; }
    public required string UnitId { get; set; }
}

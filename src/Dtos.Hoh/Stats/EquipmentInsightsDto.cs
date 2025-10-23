using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class EquipmentInsightsDto
{
    public required int FromLevel { get; init; }
    public required int ToLevel { get; init; }
    public required string UnitId { get; init; }
    public required EquipmentSlotType EquipmentSlotType { get; init; }
    public required IReadOnlyCollection<EquipmentSet> EquipmentSets { get; init; }
    public required IReadOnlyCollection<StatAttribute> MainAttributes { get; init; }
    public IReadOnlyCollection<StatAttribute> SubAttributesLevel4 { get; init; } = new List<StatAttribute>();
    public IReadOnlyCollection<StatAttribute> SubAttributesLevel8 { get; init; } = new List<StatAttribute>();
    public IReadOnlyCollection<StatAttribute> SubAttributesLevel12 { get; init; } = new List<StatAttribute>();
    public IReadOnlyCollection<StatAttribute> SubAttributesLevel16 { get; init; } = new List<StatAttribute>();
}

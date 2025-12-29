using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Calculators.Interfaces;

public interface IEquipmentStatsCalculators
{
    IReadOnlyDictionary<UnitStatType, float> Calculate(EquipmentItem? hand, EquipmentItem? garment,
        EquipmentItem? hat, EquipmentItem? neck, EquipmentItem? ring,
        IReadOnlyDictionary<UnitStatType, float> heroBaseStats,
        IReadOnlyDictionary<EquipmentSet, EquipmentSetDefinition> setDefinitions);
}

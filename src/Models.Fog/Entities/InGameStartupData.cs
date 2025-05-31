using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameStartupData
{
    public IReadOnlyCollection<HohCity>? Cities { get; init; }
    public IReadOnlyCollection<EquipmentItem>? Equipment { get; init; }
    public BasicCommandCenterProfile? Profile { get; init; }
}

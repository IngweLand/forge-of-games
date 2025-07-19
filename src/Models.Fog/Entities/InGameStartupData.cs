using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Research;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameStartupData
{
    public IReadOnlyCollection<HohCity>? Cities { get; init; }
    public IReadOnlyCollection<EquipmentItem>? Equipment { get; init; }
    public BasicCommandCenterProfile? Profile { get; init; }

    public IReadOnlyDictionary<CityId, IReadOnlyCollection<ResearchStateTechnology>> ResearchState { get; init; } =
        new Dictionary<CityId, IReadOnlyCollection<ResearchStateTechnology>>();
}

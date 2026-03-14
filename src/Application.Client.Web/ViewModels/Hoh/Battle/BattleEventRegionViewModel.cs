using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleEventRegionViewModel
{
    public required IReadOnlyCollection<BattleEventEncounterViewModel> Encounters { get; init; }
    public required string Name { get; init; }
    public required RegionId RegionId { get; init; }
}

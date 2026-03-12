using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleEventBasicViewModel
{
    public required IReadOnlyCollection<int> Encounters { get; init; }
    public required RegionId Id { get; init; }
    public required string Name { get; init; }
}

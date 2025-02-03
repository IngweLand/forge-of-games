using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroSupportUnitViewModel
{
    public required string IconUrl { get; init; }
    public required IReadOnlyCollection<IconLabelItemViewModel> StatsItems { get; init; }
    [Obsolete]
    public IReadOnlyDictionary<UnitStatType, float> Stats { get; init; }
    public required int Power { get; init; }
}

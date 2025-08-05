using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class UnitStatBreakdownViewModel
{
    public required string IconUrl { get; set; }

    public required string TotalValue { get; init; }
    public required IReadOnlyDictionary<UnitStatSource, string> Values { get; init; }
}

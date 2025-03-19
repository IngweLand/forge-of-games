using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Helpers;

public class UnitColorAdvantageSystem
{
    public static IReadOnlyDictionary<UnitColor, UnitColorAffinity> ColorAffinities =
        new List<UnitColorAffinity>()
        {
            new() {Color = UnitColor.Blue, StrongAgainst = UnitColor.Red, WeakAgainst = UnitColor.Green},
            new() {Color = UnitColor.Green, StrongAgainst = UnitColor.Blue, WeakAgainst = UnitColor.Red},
            new() {Color = UnitColor.Red, StrongAgainst = UnitColor.Green, WeakAgainst = UnitColor.Blue},
        }.ToDictionary(a => a.Color);
}
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Extensions;

public static class ExpansionCostsExtensions
{
    public static IReadOnlyCollection<int> ToPremiumExpansionCosts(this IEnumerable<ExpansionCosts> src)
    {
        return src.FirstOrDefault(x => x.UnlockingType == ExpansionUnlockingType.Premium)?.ConstructionComponents
            .Select(x => x.Cost.First().Amount).Order().ToList() ?? [];
    }
}

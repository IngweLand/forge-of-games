using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Core.Calculators.Interfaces;

public interface ICityCalculators
{
    BuildingMultilevelCost CalculateMultilevelCost(IEnumerable<BuildingLevelCostData> levelCosts,
        int currentLevel, int toLevel);

    IReadOnlyCollection<ResourceAmount> CalculateWonderLevelsCost(WonderDto wonder, int currentLevel, int targetLevel);
}

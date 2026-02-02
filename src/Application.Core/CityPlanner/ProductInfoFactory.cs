using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class ProductInfoFactory : IProductInfoFactory
{
    public ProductInfo Create(ProductionComponent productionComponent)
    {
        return new ProductInfo(productionComponent.Id,
            productionComponent.ProductionTime,
            productionComponent.Products.OfType<ResourceReward>().Select(x => x.ResourceId)
                .ToDictionary(x => x, HohStringParser.GetConcreteId));
    }
}

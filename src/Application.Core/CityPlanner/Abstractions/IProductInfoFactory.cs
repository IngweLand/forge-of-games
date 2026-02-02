using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface IProductInfoFactory
{
    ProductInfo Create(ProductionComponent productionComponent);
}

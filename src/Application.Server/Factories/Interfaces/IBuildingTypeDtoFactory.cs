using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBuildingTypeDtoFactory
{
    BuildingTypeDto Create(BuildingType buildingType, CityId cityId, ICollection<Building> buildings);
}

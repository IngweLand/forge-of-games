using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IBuildingGroupDtoFactory
{
    BuildingGroupDto Create(BuildingGroup group, BuildingType type, IEnumerable<Building> buildings);
}

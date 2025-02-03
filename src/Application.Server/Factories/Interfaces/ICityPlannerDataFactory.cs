using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CityPlanner;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface ICityPlannerDataFactory
{
    CityPlannerDataDto Create(CityDefinition cityDefinition, IReadOnlyCollection<Expansion> expansions,
        IReadOnlyCollection<BuildingDto> buildings, IEnumerable<BuildingCustomization> customizations, IReadOnlyCollection<Age> ages);
}

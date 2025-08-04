using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBuildingLevelSpecsFactory
{
    BuildingLevelSpecs Create(BuildingDto building);
    IReadOnlyCollection<BuildingLevelSpecs> Create(BuildingLevelRange levelRange);
}

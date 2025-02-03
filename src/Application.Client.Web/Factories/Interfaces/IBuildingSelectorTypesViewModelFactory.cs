using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBuildingSelectorTypesViewModelFactory
{
    BuildingSelectorTypesViewModel Create(BuildingType buildingType, IEnumerable<BuildingDto> buildings);
}

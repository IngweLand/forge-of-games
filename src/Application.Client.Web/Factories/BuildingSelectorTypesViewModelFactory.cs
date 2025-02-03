using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingSelectorTypesViewModelFactory(IMapper mapper, IBuildingTypeCssIconClassProvider buildingTypeCssIconClassProvider) : IBuildingSelectorTypesViewModelFactory
{
    public BuildingSelectorTypesViewModel Create(BuildingType buildingType, IEnumerable<BuildingDto> buildings)
    {
        var groups = buildings.DistinctBy(b => b.Group);
        return new BuildingSelectorTypesViewModel()
        {
            Icon = buildingTypeCssIconClassProvider.GetIcon(buildingType),
            BuildingGroups = mapper.Map<IReadOnlyCollection<BuildingSelectorItemViewModel>>(groups),
        };
    }
}

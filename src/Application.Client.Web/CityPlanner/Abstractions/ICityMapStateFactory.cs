using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapStateFactory
{
    CityMapState Create(IReadOnlyCollection<BuildingDto> buildings,
        IReadOnlyCollection<BuildingCustomizationDto> buildingCustomizations,
        IReadOnlyCollection<BuildingSelectorTypesViewModel> buildingSelectorItems,
        IReadOnlyCollection<AgeDto> ages,
        HohCity city,
        WonderDto? wonder);
}

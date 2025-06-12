using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapStateFactory(
    ICityMapEntityFactory cityMapEntityFactory,
    IBuildingLevelRangesFactory buildingLevelRangesFactory)
    : ICityMapStateFactory
{
    public CityMapState Create(IReadOnlyCollection<BuildingDto> buildings,
        IReadOnlyCollection<BuildingCustomizationDto> buildingCustomizations,
        IReadOnlyCollection<BuildingSelectorTypesViewModel> buildingSelectorItems,
        IReadOnlyCollection<AgeDto> ages,
        HohCity city,
        WonderDto? wonder)
    {
        var buildingDictionary = buildings.ToDictionary(b => b.Id);
        var age = ages.First(a => a.Id == city.AgeId);
        var state = new CityMapState(buildingLevelRangesFactory)
        {
            Buildings = buildingDictionary,
            BuildingCustomizations = buildingCustomizations,
            BuildingSelectorItems = buildingSelectorItems,
            CityId = city.Id,
            InGameCityId = city.InGameCityId,
            CityName = city.Name,
            CityAge = age,
            Snapshots = city.Snapshots,
            CityWonder = wonder,
            CityWonderLevel = city.WonderLevel,
        };
        state.AddRange(city.Entities.Select(hohCityMapEntity =>
        {
            var building = buildings.First(b => b.Id == hohCityMapEntity.CityEntityId);
            return cityMapEntityFactory.Create(building, hohCityMapEntity);
        }));
        return state;
    }
}

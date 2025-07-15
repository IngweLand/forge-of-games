using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class CityMapStateCoreFactory(ICityMapEntityFactory cityMapEntityFactory) : ICityMapStateCoreFactory
{
    public CityMapStateCore Create(IReadOnlyCollection<BuildingDto> buildings,
        IReadOnlyCollection<AgeDto> ages,
        HohCity city,
        IMapArea mapArea,
        WonderDto? wonder)
    {
        var buildingDictionary = buildings.ToDictionary(b => b.Id);
        var age = ages.First(a => a.Id == city.AgeId);
        var state = new CityMapStateCore(mapArea)
        {
            Buildings = buildingDictionary,
            CityName = city.Name,
            CityAge = age,
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

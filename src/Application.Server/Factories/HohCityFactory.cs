using AutoMapper;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class HohCityFactory(IMapper mapper) : IHohCityFactory
{
    public HohCity Create(City inGameCity, IReadOnlyDictionary<string, Building> buildings)
    {
        var cityHalls = buildings.Where(kvp => kvp.Value.Type == BuildingType.CityHall).Select(kvp => kvp.Value).ToList();
        var cityHallMapEntity = inGameCity.MapEntities.Single(cme => cityHalls.Any(b => b.Id == cme.CityEntityId));
        var cityHall = cityHalls.First(b => b.Id == cityHallMapEntity.CityEntityId);
        var entities = mapper.Map<IList<HohCityMapEntity>>(inGameCity.MapEntities);
        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            entity.Id = i;
            entity.Y =  -entity.Y - (entity.IsRotated?buildings[entity.CityEntityId].Width:buildings[entity.CityEntityId].Length);
        }

        return new HohCity()
        {
            Id = Guid.NewGuid().ToString(),
            InGameCityId = inGameCity.CityId,
            AgeId = cityHall.Age!.Id,
            Entities = entities,
            Name = $"Import - {inGameCity.CityId} - {DateTime.Now:g}",
        };
    }
}

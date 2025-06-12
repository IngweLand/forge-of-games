using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IHohCityFactory
{
    HohCity Create(City inGameCity, IReadOnlyDictionary<string, Building> buildings, WonderId wonderId, int wonderLevel);
}

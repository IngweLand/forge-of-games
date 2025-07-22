using System.Drawing;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface ICityMapEntityFactory
{
    CityMapEntity Create(BuildingDto building, Point location, BuildingLevelRange levelRange, int level = -1);
    CityMapEntity Create(BuildingDto building, HohCityMapEntity hohCityMapEntity);
    CityMapEntity Duplicate(CityMapEntity sourceEntity, BuildingDto building);
}

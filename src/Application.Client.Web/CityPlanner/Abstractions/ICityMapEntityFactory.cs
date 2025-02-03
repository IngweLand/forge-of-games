using System.Drawing;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityMapEntityFactory
{
    CityMapEntity Create(BuildingDto building, Point location, BuildingLevelRange levelRange, int level = -1);
    CityMapEntity Create(BuildingDto building, HohCityMapEntity hohCityMapEntity);
}

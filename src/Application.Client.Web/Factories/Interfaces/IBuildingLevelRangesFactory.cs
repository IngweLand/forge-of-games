using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBuildingLevelRangesFactory
{
    IReadOnlyDictionary<BuildingGroup, BuildingLevelRange> Create(IReadOnlyCollection<BuildingDto> buildings);
}

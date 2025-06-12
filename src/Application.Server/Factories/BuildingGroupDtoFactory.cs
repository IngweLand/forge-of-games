using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class BuildingGroupDtoFactory(IHohGameLocalizationService localizationService, IMapper mapper)
    : IBuildingGroupDtoFactory
{
    public BuildingGroupDto Create(BuildingGroup group, BuildingType type, IEnumerable<Building> buildings)
    {
        return new BuildingGroupDto
        {
            Id = group,
            Name = localizationService.GetBuildingGroup(group),
            Buildings = mapper.Map<List<BuildingDto>>(buildings),
            Type = type,
            TypeName = localizationService.GetBuildingType(type),
            CityName = localizationService.GetCityName(buildings.First().CityIds.First()),
        };
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class BuildingTypeDtoFactory(IHohGameLocalizationService localizationService, IMapper mapper)
    : IBuildingTypeDtoFactory
{
    public BuildingTypeDto Create(BuildingType buildingType, CityId cityId, ICollection<Building> buildings)
    {
        if (buildings.Count == 0)
        {
            return new BuildingTypeDto
            {
                Id = buildingType,
                Name = string.Empty,
                CityId = cityId,
                CityName = string.Empty,
                Groups = [],
            };
        }

        return new BuildingTypeDto
        {
            Id = buildingType,
            Name = localizationService.GetBuildingType(buildingType, true),
            CityId = cityId,
            CityName = localizationService.GetCityName(cityId),
            Groups = mapper.Map<IReadOnlyCollection<BuildingGroupBasicDto>>(buildings).OrderBy(b => b.Name).ToList(),
        };
    }
}

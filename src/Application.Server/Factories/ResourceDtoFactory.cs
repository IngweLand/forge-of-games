using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Factories;

public class ResourceDtoFactory(IMapper mapper, IHohGameLocalizationService gameLocalizationService) : IResourceDtoFactory
{
    public ResourceDto Create(Resource resource, Age? age)
    {
        return new ResourceDto
        {
            Id = resource.Id,
            CityIds = resource.CityIds,
            Type = resource.Type,
            Age = age != null ? mapper.Map<AgeDto>(age) : null,
            Name = gameLocalizationService.GetResourceName(resource.Id),
        };
    }
}

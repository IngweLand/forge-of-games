using AutoMapper;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CityMappingProfile:Profile
{
    public CityMappingProfile()
    {
        CreateMap<CityMapEntity, HohCityMapEntity>();
    }
}

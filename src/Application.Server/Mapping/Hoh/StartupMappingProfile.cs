using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class StartupMappingProfile :Profile
{
    public StartupMappingProfile()
    {
        CreateMap<CityMapEntityDto, CityMapEntity>()
            .ForMember(dest=> dest.CustomizationId, opt => 
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.CustomizationEntityId));
                opt.MapFrom(src => src.CustomizationEntityId);
            });
        CreateMap<CityDTO, City>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId));
    }
}

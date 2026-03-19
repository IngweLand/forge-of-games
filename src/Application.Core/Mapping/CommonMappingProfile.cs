using AutoMapper;
using Ingweland.Fog.Application.Core.Mapping.Converters;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Core.Mapping;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        CreateMap<Age, AgeDto>()
            .ForMember(dest => dest.Name, opt => opt.ConvertUsing<AgeLocalizationConverter, string>(src => src.Id));
    }
}

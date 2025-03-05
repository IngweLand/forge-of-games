using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CommonMappingProfile:Profile
{
    public CommonMappingProfile()
    {
        CreateMap<Age, AgeDto>()
            .ForMember(dest=> dest.Name, opt => opt.ConvertUsing<AgeLocalizationConverter, string>(src => src.Id));

        CreateMap<WikipediaResponse, WikipediaResponseDto>()
            .ForMember(dest => dest.DesktopUrl, opt =>
            {
                opt.PreCondition(src => src?.ContentUrls?.Desktop?.Page != null);
                opt.MapFrom(src => src!.ContentUrls!.Desktop!.Page);
            })
            .ForMember(dest => dest.MobileUrl, opt =>
            {
                opt.PreCondition(src => src?.ContentUrls?.Mobile?.Page != null);
                opt.MapFrom(src => src!.ContentUrls!.Mobile!.Page);
            });
    }
}

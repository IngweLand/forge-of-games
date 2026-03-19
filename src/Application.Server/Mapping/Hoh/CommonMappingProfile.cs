using AutoMapper;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
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

        CreateMap<InGameEventEntity, InGameEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.EventId));
    }
}

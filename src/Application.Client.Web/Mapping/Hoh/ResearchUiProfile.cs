using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Dtos.Hoh.Research;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class ResearchUiProfile:Profile
{
    public ResearchUiProfile()
    {
        CreateMap<TechnologyDto, ResearchCalculatorTechnologyViewModel>()
            .ForMember(dest => dest.HorizontalIndex, opt => opt.MapFrom(src => src.HorizontalIndex))
            .ForMember(dest => dest.VerticalIndex, opt => opt.MapFrom(src => src.VerticalIndex))
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<TechnologyIdToIconUrlConverter, string>(src => src.Id));
        
    }
}

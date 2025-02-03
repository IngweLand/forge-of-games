using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Dtos.Hoh.Research;
using Ingweland.Fog.Models.Hoh.Entities.Research;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class ResearchMappingProfile : Profile
{
    public ResearchMappingProfile()
    {
        CreateMap<Technology, TechnologyDto>()
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<TechnologyNameLocalizationConverter, string>(src => src.Id))
            .ForMember(dest => dest.Costs, opt => opt.MapFrom(src => src.ResearchComponent.Costs))
            .ForMember(dest => dest.ParentTechnologies,
                opt => opt.MapFrom(src => src.ResearchComponent.ParentTechnologies));
    }
}

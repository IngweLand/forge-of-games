using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CommonUiProfile : Profile
{
    public CommonUiProfile()
    {
        CreateMap<ResourceAmount, IconLabelItemViewModel>()
            .ForMember(dst => dst.Label, opt => opt.MapFrom(src => src.Amount.ToString("N0")))
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<ResourceIdToIconUrlConverter, string>(src => src.ResourceId));

        CreateMap<AgeDto, AgeViewModel>()
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ToCssColor()));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class UnitUiProfile : Profile
{
    public UnitUiProfile()
    {
        CreateMap<HeroBasicDto, HeroBasicViewModel>().ConvertUsing<HeroBasicDtoToViewModelConverter>();
        CreateMap<HeroDto, HeroBasicViewModel>().ConvertUsing<HeroDtoToHeroBasicViewModelConverter>();
        CreateMap<HeroDto, HeroViewModel>().ConvertUsing<HeroDtoToViewModelConverter>();
        CreateMap<HeroProgressionCostResource, IReadOnlyCollection<IconLabelItemViewModel>>()
            .ConvertUsing<HeroProgressionCostResourceToResourceAmountsConverter>();
    }
}

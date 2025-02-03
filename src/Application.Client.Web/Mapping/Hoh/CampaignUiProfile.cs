using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CampaignUiProfile : Profile
{
    public CampaignUiProfile()
    {
        CreateMap<RewardBase, IconLabelItemViewModel>()
            .Include<ResourceReward, IconLabelItemViewModel>()
            .Include<IncreaseExpansionRightReward, IconLabelItemViewModel>();
        CreateMap<RewardBase, IList<EncounterRewardViewModel>>()
            .ConvertUsing<EncounterRewardViewModelConverter>();

        CreateMap<ResourceReward, IconLabelItemViewModel>()
            .ForMember(dst => dst.Label, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<ResourceIdToIconUrlConverter, string>(src => src.ResourceId));
        CreateMap<IncreaseExpansionRightReward, IconLabelItemViewModel>()
            .ForMember(dst => dst.Label, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<CityIdToExpansionIconUrlConverter, CityId>(src => src.CityId));
        CreateMap<ResourceReward, EncounterRewardViewModel>()
            .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dst => dst.Subtitle, opt => opt.Ignore())
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<ResourceIdToIconUrlConverter, string>(src => src.ResourceId));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class TreasureHuntUiProfile : Profile
{
    public TreasureHuntUiProfile()
    {
        CreateMap<TreasureHuntDifficultyDataBasicDto, TreasureHuntDifficultyBasicViewModel>()
            .ForMember(dst => dst.IconUrl,
                opt => opt.ConvertUsing<TreasureHuntDifficultyToIconUrlConverter, int>(src => src.Difficulty));
    }
}

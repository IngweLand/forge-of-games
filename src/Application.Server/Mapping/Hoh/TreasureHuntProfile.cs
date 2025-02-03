using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class TreasureHuntProfile : Profile
{
    public TreasureHuntProfile()
    {
        CreateMap<TreasureHuntDifficultyData, TreasureHuntDifficultyDataBasicDto>()
            .ForMember(dst => dst.Name,
                opt => opt.ConvertUsing<TreasureHuntDifficultyLocalizationConverter, int>(src => src.Difficulty));
        CreateMap<TreasureHuntStage, TreasureHuntStageBasicDto>()
            .ForMember(dst => dst.Name,
                opt => opt.ConvertUsing<TreasureHuntStageNameLocalizationConverter, int>(src => src.Index));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Core.Mapping.Converters;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Core.Mapping;

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

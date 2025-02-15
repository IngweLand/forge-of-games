using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;
using AllianceRankingType = Ingweland.Fog.Models.Hoh.Enums.AllianceRankingType;
using PlayerRankingType = Ingweland.Fog.Models.Hoh.Enums.PlayerRankingType;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class InGameDataMappingProfile :Profile
{
    public InGameDataMappingProfile()
    {
        CreateMap<CityMapEntityDto, CityMapEntity>()
            .ForMember(dest=> dest.CustomizationId, opt => 
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.CustomizationEntityId));
                opt.MapFrom(src => src.CustomizationEntityId);
            });
        CreateMap<CityDTO, City>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId));

        CreateMap<PlayerRank, Player>()
            .ForMember(dest => dest.InGamePlayerId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RankingPoints, opt => opt.MapFrom(src => src.Points))
            .ForMember(dest => dest.UpdatedAt, opt =>
                opt.MapFrom((_, _, _, context) => context.Items.GetRequiredItem<DateOnly>(ResolutionContextKeys.DATE)))
            .ForMember(dest => dest.WorldId, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<string>(ResolutionContextKeys.WORLD_ID)));

        CreateMap<PlayerRank, PlayerRanking>()
            .ForMember(dest => dest.InGamePlayerId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<PlayerRankingType>(ResolutionContextKeys.PLAYER_RANKING_TYPE)))
            .ForMember(dest => dest.CollectedAt, opt =>
                opt.MapFrom((_, _, _, context) => context.Items.GetRequiredItem<DateOnly>(ResolutionContextKeys.DATE)))
            .ForMember(dest => dest.WorldId, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<string>(ResolutionContextKeys.WORLD_ID)));
        
        CreateMap<AllianceRank, Alliance>()
            .ForMember(dest => dest.InGameAllianceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RankingPoints, opt => opt.MapFrom(src => src.Points))
            .ForMember(dest => dest.UpdatedAt, opt =>
                opt.MapFrom((_, _, _, context) => context.Items.GetRequiredItem<DateOnly>(ResolutionContextKeys.DATE)))
            .ForMember(dest => dest.WorldId, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<string>(ResolutionContextKeys.WORLD_ID)));

        CreateMap<AllianceRank, AllianceRanking>()
            .ForMember(dest => dest.InGameAllianceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<AllianceRankingType>(ResolutionContextKeys.ALLIANCE_RANKING_TYPE)))
            .ForMember(dest => dest.CollectedAt, opt =>
                opt.MapFrom((_, _, _, context) => context.Items.GetRequiredItem<DateOnly>(ResolutionContextKeys.DATE)))
            .ForMember(dest => dest.WorldId, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<string>(ResolutionContextKeys.WORLD_ID)));
    }
}

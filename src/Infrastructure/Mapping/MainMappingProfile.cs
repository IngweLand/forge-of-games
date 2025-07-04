using AutoMapper;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Infrastructure.Mapping;

public class MainMappingProfile : Profile
{
    public MainMappingProfile()
    {
        CreateMap<InGameStartupData, InGameStartupDataTableEntity>()
            .ForMember(dest => dest.CitiesData, opt => opt.Ignore())
            .ForMember(dest => dest.ProfileData, opt => opt.Ignore())
            .ForMember(dest => dest.RelicsJson, opt => opt.Ignore())
            .ForMember(dest => dest.EquipmentData, opt => opt.Ignore());
        CreateMap<InGameStartupDataTableEntity, InGameStartupData>();

        CreateMap<PlayerRank, PlayerRankingTableEntity>()
            .ForMember(dest => dest.Type, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<PlayerRankingType>(ResolutionContextKeys.PLAYER_RANKING_TYPE)))
            .ReverseMap();

        CreateMap<AllianceRank, AllianceRankingTableEntity>()
            .ForMember(dest => dest.Type, opt =>
                opt.MapFrom((_, _, _, context) =>
                    context.Items.GetRequiredItem<AllianceRankingType>(ResolutionContextKeys.ALLIANCE_RANKING_TYPE)))
            .ReverseMap();

        CreateMap<InGameRawData, InGameRawDataTableEntity>()
            .ForMember(dest => dest.CompressedData, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<InGameBinData, InGameBinDataTableEntity>()
            .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.DataType))
            .ForMember(dest => dest.RowKey,
                opt => opt.MapFrom(src =>
                    $"{src.GameWorldId}_{src.PlayerId}_{DateOnly.FromDateTime(src.CollectedAt).ToString("O")}"));

        CreateMap<InGameBinDataTableEntity, InGameBinData>()
            .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.PartitionKey));
    }
}

using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;
using Ingweland.Fog.Shared.Formatters;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class StatsHubUiProfile : Profile
{
    public StatsHubUiProfile()
    {
        CreateMap<PlayerDto, PlayerViewModel>()
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank == 0 ? "-" : src.Rank.ToString()))
            .ForMember(dest => dest.RankingPoints,
                opt => opt.MapFrom(src => src.RankingPoints == 0 ? "-" : src.RankingPoints.ToString()))
            .ForMember(dest => dest.RankingPointsFormatted,
                opt => opt.MapFrom(src =>
                    src.RankingPoints == 0 ? "-" : NumberFormatter.FormatCompactNumber(src.RankingPoints)))
            .ForMember(dest => dest.Age, opt =>
                opt.MapFrom((src, _, _, context) =>
                {
                    var ages = context.Items.GetRequiredItem<IReadOnlyDictionary<string, AgeDto>>(ResolutionContextKeys
                        .AGES);
                    return ages[src.Age].Name;
                }))
            .ForMember(dest => dest.AgeColor, opt =>
                opt.MapFrom((src, _, _, context) =>
                {
                    var ages = context.Items.GetRequiredItem<IReadOnlyDictionary<string, AgeDto>>(ResolutionContextKeys
                        .AGES);
                    return ages[src.Age].ToCssColor();
                }))
            .ForMember(dest => dest.AvatarUrl,
                opt => opt.ConvertUsing<PlayerAvatarIdToUrlConverter, int>(src => src.AvatarId))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("d")))
            .ForMember(dest => dest.IsStale,
                opt => opt.MapFrom(src => src.UpdatedAt < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1)));
        CreateMap<PaginatedList<PlayerDto>, PaginatedList<PlayerViewModel>>();

        CreateMap<AllianceMemberDto, PlayerViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlayerId))
            .ForMember(dest => dest.RankingPoints,
                opt => opt.MapFrom(src => src.RankingPoints == 0 ? "-" : src.RankingPoints.ToString()))
            .ForMember(dest => dest.RankingPointsFormatted,
                opt => opt.MapFrom(src =>
                    src.RankingPoints == 0 ? "-" : NumberFormatter.FormatCompactNumber(src.RankingPoints)))
            .ForMember(dest => dest.Age, opt =>
                opt.MapFrom((src, _, _, context) =>
                {
                    var ages = context.Items.GetRequiredItem<IReadOnlyDictionary<string, AgeDto>>(ResolutionContextKeys
                        .AGES);
                    return ages[src.Age].Name;
                }))
            .ForMember(dest => dest.AgeColor, opt =>
                opt.MapFrom((src, _, _, context) =>
                {
                    var ages = context.Items.GetRequiredItem<IReadOnlyDictionary<string, AgeDto>>(ResolutionContextKeys
                        .AGES);
                    return ages[src.Age].ToCssColor();
                }))
            .ForMember(dest => dest.AvatarUrl,
                opt => opt.ConvertUsing<PlayerAvatarIdToUrlConverter, int>(src => src.AvatarId))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("d")))
            .ForMember(dest => dest.IsStale,
                opt => opt.MapFrom(src => src.UpdatedAt < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1)));

        CreateMap<AllianceDto, AllianceViewModel>()
            .ForMember(dest => dest.RankingPointsFormatted,
                opt => opt.MapFrom(src => NumberFormatter.FormatCompactNumber(src.RankingPoints)))
            .ForMember(dest => dest.AvatarIconUrl,
                opt => opt.ConvertUsing<AllianceAvatarIconIdToUrlConverter, int>(src => src.AvatarIconId))
            .ForMember(dest => dest.AvatarBackgroundUrl,
                opt => opt.ConvertUsing<AllianceAvatarBackgroundIdToUrlConverter, int>(src => src.AvatarBackgroundId))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("d")))
            .ForMember(dest => dest.IsStale,
                opt => opt.MapFrom(src => src.UpdatedAt < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1)));
        CreateMap<PaginatedList<AllianceDto>, PaginatedList<AllianceViewModel>>();
        CreateMap<AllianceWithRankings, AllianceWithRankingsViewModel>()
            .ForMember(dest => dest.RegisteredAt, opt =>
            {
                opt.PreCondition(src => src.RegisteredAt != null);
                opt.MapFrom(src => src.RegisteredAt!.Value.ToString("d"));
            })
            .ForMember(dest => dest.LeaderName, opt =>
            {
                opt.PreCondition(src =>
                    src.CurrentMembers.FirstOrDefault(x => x.Role == AllianceMemberRole.AllianceLeader) != null);
                opt.MapFrom(src => src.CurrentMembers.First(x => x.Role == AllianceMemberRole.AllianceLeader).Name);
            })
            .ForMember(dest => dest.CurrentMembers,
                opt => opt.MapFrom(src => src.CurrentMembers.OrderByDescending(x => x.RankingPoints)));
    }
}

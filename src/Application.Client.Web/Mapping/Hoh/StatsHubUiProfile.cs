using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class StatsHubUiProfile : Profile
{
    public StatsHubUiProfile()
    {
        CreateMap<PlayerDto, PlayerViewModel>()
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
        CreateMap<PlayerWithRankings, PlayerWithRankingsViewModel>()
            .ForMember(dest => dest.Ages, opt =>
                opt.MapFrom((src, _, _, context) =>
                {
                    var ages = context.Items.GetRequiredItem<IReadOnlyDictionary<string, AgeDto>>(ResolutionContextKeys
                        .AGES);
                    return src.Ages.Select(
                        a => new StatsTimedStringValue() {Date = a.Date, Value = ages[a.Value].Name});
                }));
    }
}

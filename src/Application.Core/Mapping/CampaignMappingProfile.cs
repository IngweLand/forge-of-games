using AutoMapper;
using Ingweland.Fog.Application.Core.Mapping.Converters;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Mapping;

public class CampaignMappingProfile : Profile
{
    public CampaignMappingProfile()
    {
        CreateMap<Continent, ContinentBasicDto>()
            .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => Enumerable.OrderBy(src.Regions, r => r.Index)))
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<ContinentNameLocalizationConverter, ContinentId>(src => src.Id));
        CreateMap<Region, RegionBasicDto>()
            .ForMember(dest => dest.Name,
                opt => opt.ConvertUsing<RegionNameLocalizationConverter, RegionId>(src => src.Id));
        CreateMap<Encounter, EncounterDto>();
        CreateMap<EncounterDetails, EncounterDetailsDto>()
            .ForMember(dest => dest.Waves,
                opt =>
                {
                    opt.PreCondition(src => src.BattleDetails != null);
                    opt.MapFrom(src => Enumerable.OrderBy<BattleWave, string>(src.BattleDetails!.Waves, w => w.Id));
                })
            .ForMember(dest => dest.AvailableHeroSlots,
                opt =>
                {
                    opt.PreCondition(src => src.BattleDetails != null);
                    opt.MapFrom(src => 5 - src.BattleDetails!.DisabledPlayerSlotIds.Count);
                })
            .ForMember(dest => dest.RequiredHeroClasses,
                opt =>
                {
                    opt.PreCondition(src => src.BattleDetails != null);
                    opt.MapFrom(src => src.BattleDetails!.RequiredHeroClasses);
                })
            .ForMember(dest => dest.RequiredHeroTypes,
                opt =>
                {
                    opt.PreCondition(src => src.BattleDetails != null);
                    opt.MapFrom(src => src.BattleDetails!.RequiredHeroTypes);
                });
    }
}
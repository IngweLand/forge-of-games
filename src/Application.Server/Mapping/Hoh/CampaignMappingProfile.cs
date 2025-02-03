using AutoMapper;
using Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

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
        CreateMap<Encounter, EncounterDto>()
            .ForMember(dest => dest.Waves,
                opt => opt.MapFrom(src => Enumerable.OrderBy<BattleWave, string>(src.BattleDetails.Waves, w => w.Id)))
            .ForMember(dest=> dest.AvailableHeroSlots, opt => opt.MapFrom(src => 5 - src.BattleDetails.DisabledPlayerSlotIds.Count));
    }
}

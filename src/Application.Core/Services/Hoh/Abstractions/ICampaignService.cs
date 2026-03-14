using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICampaignService
{
    [Get(FogUrlBuilder.ApiRoutes.CAMPAIGN_CONTINENTS_BASIC_DATA_PATH)]
    Task<IReadOnlyCollection<ContinentBasicDto>> GetCampaignContinentsBasicDataAsync();

    [Get(FogUrlBuilder.ApiRoutes.CAMPAIGN_REGION_TEMPLATE)]
    Task<RegionDto?> GetRegionAsync(RegionId regionId);

    [Get(FogUrlBuilder.ApiRoutes.CAMPAIGN_REGION_BASIC_DATA_TEMPLATE)]
    Task<RegionBasicDto> GetRegionBasicDataAsync(RegionId regionId);

    [Get(FogUrlBuilder.ApiRoutes.BATTLE_EVENTS_BASIC_DATA)]
    Task<IReadOnlyCollection<BattleEventBasicDto>> GetBattleEventsBasicDataAsync();

    [Get(FogUrlBuilder.ApiRoutes.BATTLE_EVENT_REGION_TEMPLATE)]
    Task<BattleEventRegionDto?> GetBattleEventRegionAsync(RegionId regionId);
}

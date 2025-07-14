using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICampaignService
{
    Task<IReadOnlyCollection<ContinentBasicDto>> GetCampaignContinentsBasicDataAsync();

    Task<RegionDto?> GetRegionAsync(RegionId regionId);

    Task<RegionBasicDto> GetRegionBasicDataAsync(RegionId regionId);
}

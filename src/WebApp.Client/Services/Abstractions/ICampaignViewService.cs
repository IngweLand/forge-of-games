using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface ICampaignViewService
{
    Task<Region?> GetRegion(RegionId id);
}

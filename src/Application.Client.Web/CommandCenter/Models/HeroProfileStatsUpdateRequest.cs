using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class HeroProfileStatsUpdateRequest
{
    public required int AbilityLevel { get; set; }
    public required int AwakeningLevel { get; set; }
    public required int BarracksLevel { get; set; }
    public required string HeroProfileId { get; init; }
    public required HeroLevelSpecs Level { get; set; }
}

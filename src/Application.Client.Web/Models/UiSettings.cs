using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class UiSettings
{
    public Difficulty CampaignDifficulty { get; set; }
    public bool CityViewerIntroMessageViewed { get; set; }
    public bool ShowBattleTimelineRelics { get; set; } = true;
    public bool ShowHeroVideoAvatar { get; set; }
}

using Ingweland.Fog.Application.Client.Core.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class StatsHubPage : StatsHubPageBase
{
    private const string MAIN_WORLD_ID = "un1";
    private const string BETA_WORLD_ID = "zz1";

    private readonly Dictionary<string, string> _worldIdToTitleMap = new()
    {
        {MAIN_WORLD_ID, FogResource.StatsHub_Worlds_MainWorld},
        {BETA_WORLD_ID, FogResource.StatsHub_Worlds_BetaWorld},
    };

    private string _selectedWorldId = MAIN_WORLD_ID;
}

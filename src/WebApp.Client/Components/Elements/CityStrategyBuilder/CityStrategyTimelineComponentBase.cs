using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.CityStrategyBuilder;

public class CityStrategyTimelineComponentBase : ComponentBase
{
    protected static string GetTypeIcon(CityStrategyTimelineItemType type)
    {
        return type switch
        {
            CityStrategyTimelineItemType.Research => "material-symbols-outlined/experiment",
            CityStrategyTimelineItemType.Description => "material-symbols-outlined/description",
            CityStrategyTimelineItemType.Layout => "material-symbols-outlined/dashboard_2",
            CityStrategyTimelineItemType.Intro => "material-symbols-outlined/tour",
            _ => string.Empty,
        };
    }
}

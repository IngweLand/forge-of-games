using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class CityPlannerNavigationState
{
    public CityPlannerNavigationStateData? Data { get; set; }

    public record CityPlannerNavigationStateData
    {
        public required HohCity City { get; init; }
        public bool IsRemote { get; init; }
    }
}

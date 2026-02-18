using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class CityStrategyNavigationState
{
    public CityStrategyNavigationStateData? Data { get; set; }

    public record CityStrategyNavigationStateData
    {
        public bool IsCommunity { get; init; }
        public bool IsRemote { get; init; }
        public required CityStrategy Strategy { get; init; }
    }
}

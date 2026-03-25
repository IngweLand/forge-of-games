namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapEntityProductViewModel
{
    public bool IsSelected { get; set; }
    public required string ProductId { get; init; }
    public required IReadOnlyCollection<CityMapEntityProductRewardViewModel> Rewards { get; init; }
}

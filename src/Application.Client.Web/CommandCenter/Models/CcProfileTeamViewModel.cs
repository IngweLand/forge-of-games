namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class CcProfileTeamViewModel
{
    public IList<HeroProfileViewModel> Heroes { get; init; } = new List<HeroProfileViewModel>();
    public required string Id { get; init; }
    public required string Name { get; set; }
    public required string Power { get; set; }
}

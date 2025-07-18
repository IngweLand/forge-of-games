namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class CcProfileViewModel
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public IReadOnlyCollection<CcProfileTeamViewModel> Teams { get; init; } = new List<CcProfileTeamViewModel>();
}

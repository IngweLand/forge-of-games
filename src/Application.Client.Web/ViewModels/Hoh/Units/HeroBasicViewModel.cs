namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroBasicViewModel
{
    public required string Id { get; set; }

    public required string Name { get; set; }
    public required string PortraitUrl { get; init; }

    public required string UnitColor { get; set; }

    public required string UnitTypeIconUrl { get; init; }
}

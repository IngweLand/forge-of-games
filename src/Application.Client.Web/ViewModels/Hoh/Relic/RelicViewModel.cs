namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;

public class RelicViewModel
{
    public bool Ascension { get; set; }
    public int AscensionLevel { get; set; }
    public required string Description { get; init; }
    public required string IconUrl { get; init; }
    public required string Id { get; init; }
    public required string Level { get; init; }
    public required string Name { get; init; }
    public required int StarCount { get; init; }
}

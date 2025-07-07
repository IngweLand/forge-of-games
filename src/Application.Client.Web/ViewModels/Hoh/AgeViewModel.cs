namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

public class AgeViewModel
{
    public static readonly AgeViewModel Blank = new()
    {
        Id = string.Empty,
        Color = "transparent",
        Name = string.Empty,
        Index = 0,
    };

    public required string Color { get; init; }
    public required string Id { get; init; }
    public required int Index { get; init; }
    public required string Name { get; init; }
}

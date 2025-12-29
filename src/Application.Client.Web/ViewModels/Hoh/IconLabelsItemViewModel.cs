namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

public class IconLabelsItemViewModel
{
    public static readonly IconLabelsItemViewModel Blank = new()
    {
        IconUrl = string.Empty,
        Label = string.Empty,
        Label2 = string.Empty,
    };

    public required string IconUrl { get; init; }
    public required string Label { get; init; }
    public required string Label2 { get; init; }
}

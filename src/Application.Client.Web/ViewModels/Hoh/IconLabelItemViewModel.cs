namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

public class IconLabelItemViewModel
{
    public static readonly IconLabelItemViewModel Blank = new()
    {
        IconUrl = string.Empty,
        Label = string.Empty,
    };

    public required string IconUrl { get; init; }
    public required string Label { get; init; }
}

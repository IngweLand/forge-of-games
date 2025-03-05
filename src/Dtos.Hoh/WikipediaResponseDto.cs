namespace Ingweland.Fog.Dtos.Hoh;

public class WikipediaResponseDto
{
    public string? DesktopUrl { get; init; }
    public required string Extract { get; init; }
    public string? MobileUrl { get; init; }
}
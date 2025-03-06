namespace Ingweland.Fog.Dtos.Hoh;

public class HohHelperResponseDto
{
    public required string? Base64ResponseData { get; init; }
    public required string ResponseUrl { get; init; }
    public required IReadOnlyCollection<string> CollectionCategoryIds { get; init; } = [];
}
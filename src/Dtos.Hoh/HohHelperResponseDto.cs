namespace Ingweland.Fog.Dtos.Hoh;

public class HohHelperResponseDto
{
    public string? Base64RequestData { get; init; }
    public string? Base64ResponseData { get; init; }
    public required IReadOnlyCollection<string> CollectionCategoryIds { get; init; } = [];
    public required string ResponseUrl { get; init; }
    public string? SubmissionId { get; init; }
}

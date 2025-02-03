namespace Ingweland.Fog.Dtos.Hoh;

public record ResourceCreatedResponse()
{
    public required string ApiResourceUrl { get; init; }
    public required string ResourceId { get; init; }
    public required string WebResourceUrl { get; init; }
}

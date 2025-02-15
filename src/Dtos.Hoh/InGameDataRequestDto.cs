using System.Text.Json.Serialization;

namespace Ingweland.Fog.Dtos.Hoh;

public class InGameDataRequestDto
{
    public required string Data { get; init; }

    public required string Url { get; init; }
}

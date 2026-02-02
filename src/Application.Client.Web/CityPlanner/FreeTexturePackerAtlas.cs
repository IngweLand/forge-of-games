using System.Text.Json.Serialization;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class FreeTexturePackerAtlas
{
    [JsonPropertyName("frames")]
    public required IReadOnlyDictionary<string, Sprite> Sprites { get; init; }

    public record Sprite
    {
        public required Frame Frame { get; init; }
    }

    public record Frame
    {
        public int H { get; init; }
        public int W { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
    }
}

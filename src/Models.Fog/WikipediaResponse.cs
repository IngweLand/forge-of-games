using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog;

public class WikipediaResponse
{
    [JsonPropertyName("content_urls")]
    public ContentUrls? ContentUrls { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("description_source")]
    public string? DescriptionSource { get; set; }

    [JsonPropertyName("dir")]
    public string? Dir { get; set; }

    [JsonPropertyName("displaytitle")]
    public string? DisplayTitle { get; set; }

    [JsonPropertyName("extract")]
    public string? Extract { get; set; }

    [JsonPropertyName("extract_html")]
    public string? ExtractHtml { get; set; }

    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    [JsonPropertyName("namespace")]
    public NamespaceInfo? Namespace { get; set; }

    [JsonPropertyName("originalimage")]
    public ImageInfo? OriginalImage { get; set; }

    [JsonPropertyName("pageid")]
    public int PageId { get; set; }

    [JsonPropertyName("revision")]
    public string? Revision { get; set; }

    [JsonPropertyName("thumbnail")]
    public ImageInfo? Thumbnail { get; set; }

    [JsonPropertyName("tid")]
    public string? Tid { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("titles")]
    public TitlesInfo? Titles { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class ImageInfo
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }
}

public class ContentUrls
{
    [JsonPropertyName("desktop")]
    public UrlInfo? Desktop { get; set; }

    [JsonPropertyName("mobile")]
    public UrlInfo? Mobile { get; set; }
}

public class UrlInfo
{
    [JsonPropertyName("edit")]
    public string? Edit { get; set; }

    [JsonPropertyName("page")]
    public string? Page { get; set; }

    [JsonPropertyName("revisions")]
    public string? Revisions { get; set; }

    [JsonPropertyName("talk")]
    public string? Talk { get; set; }
}

public class NamespaceInfo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class TitlesInfo
{
    [JsonPropertyName("canonical")]
    public string? Canonical { get; set; }

    [JsonPropertyName("display")]
    public string? Display { get; set; }

    [JsonPropertyName("normalized")]
    public string? Normalized { get; set; }
}

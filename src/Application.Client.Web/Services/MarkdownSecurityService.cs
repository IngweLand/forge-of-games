using Ganss.Xss;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Markdig;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class MarkdownSecurityService : IMarkdownSecurityService
{
    private readonly MarkdownPipeline _pipeline;
    private readonly HtmlSanitizer _sanitizer;

    public MarkdownSecurityService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .DisableHtml()
            .UseAdvancedExtensions()
            .Build();

        _sanitizer = new HtmlSanitizer();

        // Whitelist safe tags
        _sanitizer.AllowedTags.Clear();
        _sanitizer.AllowedTags.UnionWith([
            "p", "br", "strong", "em", "u", "s", "code", "pre", "hr",
            "h1", "h2", "h3", "h4", "h5", "h6",
            "ul", "ol", "li",
            "blockquote",
            "a", "img",
            "table", "thead", "tbody", "tr", "td", "th",
        ]);

        // Whitelist safe attributes
        _sanitizer.AllowedAttributes.Clear();
        _sanitizer.AllowedAttributes.Add("href");
        _sanitizer.AllowedAttributes.Add("src");
        _sanitizer.AllowedAttributes.Add("alt");
        _sanitizer.AllowedAttributes.Add("title");

        // Whitelist ONLY safe URL schemes
        _sanitizer.AllowedSchemes.Clear();
        _sanitizer.AllowedSchemes.Add("https");

        // Additional security settings
        _sanitizer.AllowedCssProperties.Clear(); // No inline styles
        _sanitizer.AllowDataAttributes = false; // No data-* attributes
    }

    public string ConvertToSafeHtml(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        var html = Markdown.ToHtml(markdown, _pipeline);

        var safeHtml = _sanitizer.Sanitize(html);

        return safeHtml;
    }
}

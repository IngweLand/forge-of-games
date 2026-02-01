using System.Globalization;
using System.Net.Mime;
using FluentResults;
using FluentResults.Extensions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

public class CityGuideMarkdownConverter(
    ICityPlannerDataConverter cityPlannerDataConverter,
    IFogSharingUiService fogSharingUiService,
    IAssetUrlProvider assetUrlProvider,
    ICityPlanner cityPlanner,
    ISharedImageUploaderService imageUploaderService) : ICityGuideMarkdownConverter
{
    private const string LAYOUT_PREFIX = "layout";
    private const int JPEG_QUALITY = 60;

    private static readonly IReadOnlySet<string> CityPlannerHosts =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {"teamtesla.tools", "heroesofhistory.wiki"};

    public async Task<Result<string>> Convert(string markdown, WonderId wonderId, string fogBaseUrl,
        IProgress<string>? progress = null)
    {
        progress?.Report(CreateProgressMessage("Parsing markdown..."));
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var documentResult = Result.Try(() => Markdown.Parse(markdown));
        if (documentResult.IsFailed)
        {
            return documentResult.ToResult();
        }

        var document = documentResult.Value;
        progress?.Report(CreateProgressMessage("Parsed."));

        var layoutLinks = document
            .Descendants<LinkInline>()
            .Where(x => !x.IsImage && !x.Any(child => child is LinkInline {IsImage: true}))
            .Where(x => CityPlannerHosts.Any(y => x.Url!.Contains(y)))
            .ToList();
        foreach (var link in layoutLinks)
        {
            link.Remove();
        }

        progress?.Report(CreateProgressMessage($"Removed {layoutLinks.Count} layout links."));

        var images = document
            .Descendants<LinkInline>()
            .Where(x => x.IsImage)
            .ToList();
        progress?.Report(CreateProgressMessage($"Found {images.Count} images."));

        foreach (var imageInline in images)
        {
            var altText = GetAltText(imageInline);
            if (altText == "header")
            {
                UpdateImageUrl(imageInline, assetUrlProvider.GetHohImageUrl(wonderId.GetImageFileName()));
            }

            if (imageInline.Parent is not LinkInline layoutLink)
            {
                continue;
            }

            progress?.Report(CreateProgressMessage($"Processing layout link: {layoutLink.Url!}"));

            var compressedLayout = cityPlannerDataConverter.ParseUrl(layoutLink.Url!);
            if (compressedLayout.IsFailed)
            {
                progress?.Report(CreateProgressMessage("Failed to parse layout."));
                return compressedLayout.ToResult();
            }

            var cityName = LAYOUT_PREFIX;
            if (!string.IsNullOrWhiteSpace(altText))
            {
                cityName = altText.StartsWith(LAYOUT_PREFIX, StringComparison.Ordinal)
                    ? altText[LAYOUT_PREFIX.Length..].Trim()
                    : altText.Trim();
            }

            var city = await cityPlannerDataConverter.ConvertAsync(compressedLayout.Value, cityName);
            if (city.IsFailed)
            {
                progress?.Report(CreateProgressMessage("Failed to convert layout."));
                return city.ToResult();
            }

            var sharingResult = await Result.Try(() => fogSharingUiService.CreateSharedData(city.Value))
                .Bind(sharedData => Result.Try(() => fogSharingUiService.ShareAsync(sharedData)));
            if (sharingResult.IsFailed)
            {
                progress?.Report(CreateProgressMessage("Failed to create shared layout."));
                return sharingResult.ToResult();
            }

            var sharedLayoutUrl = fogBaseUrl.Replace("{shareId}", sharingResult.Value.Id);

            progress?.Report(CreateProgressMessage($"Created shared layout: {sharedLayoutUrl}"));

            layoutLink.Url = sharedLayoutUrl;

            progress?.Report(CreateProgressMessage("Generating city image..."));

            var cityImage = await GenerateCityImage(city.Value);
            if (cityImage.IsFailed)
            {
                progress?.Report(CreateProgressMessage("Failed to generate city image."));
                return cityImage.ToResult();
            }

            progress?.Report(CreateProgressMessage("Uploading city image..."));

            var uploadResult = await UploadImageAsync(cityImage.Value);
            if (uploadResult.IsFailed)
            {
                progress?.Report(CreateProgressMessage("Failed to upload city image."));
                return uploadResult.ToResult();
            }

            UpdateImageUrl(imageInline, uploadResult.Value.Url);

            progress?.Report(CreateProgressMessage("Processed."));
        }

        var writer = new StringWriter();
        var renderer = new NormalizeRenderer(writer);
        pipeline.Setup(renderer);
        progress?.Report(CreateProgressMessage("Creating output markdown..."));
        var result = Result.Try(() =>
        {
            renderer.Render(document);
            return writer.ToString();
        });
        await writer.FlushAsync();
        progress?.Report(CreateProgressMessage("Successfully created output markdown."));
        return result;
    }

    private static void UpdateImageUrl(LinkInline imageInline, string imageUrl)
    {
        if (imageInline.Reference != null)
        {
            imageInline.Reference.Url = imageUrl;
        }
        else
        {
            imageInline.Url = imageUrl;
        }
    }

    private Task<Result<ImageUploadResultDto>> UploadImageAsync(byte[] imageData)
    {
        var uploadData = new ImageUploadDto
        {
            Data = imageData,
            ContentType = MediaTypeNames.Image.Jpeg,
        };
        return Result.Try(() => imageUploaderService.UploadAsync(uploadData));
    }

    private Task<Result<byte[]>> GenerateCityImage(HohCity city)
    {
        return Result.Try(() => cityPlanner.InitializeAsync(city))
            .Bind(() => Task.FromResult(cityPlanner.GenerateCityImage(SKEncodedImageFormat.Jpeg, JPEG_QUALITY)));
    }

    private static string GetAltText(LinkInline imageInline)
    {
        var altText = (imageInline.FirstChild as LiteralInline)?.Content.ToString()
            ?? imageInline.FirstChild?.ToString()
            ?? string.Empty;
        return altText;
    }

    private static string CreateProgressMessage(string src)
    {
        return $"[{DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)}] {src}";
    }
}

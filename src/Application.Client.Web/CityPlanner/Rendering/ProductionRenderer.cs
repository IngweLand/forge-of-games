using System.Net.Http.Json;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class ProductionRenderer : IProductionRenderer
{
    private const int ICON_SIZE = 24;
    private const int GAP = 4;
    private const float GRAYSCALE_BRIGHTNESS = 0.2f;
    
    private readonly IAssetUrlProvider _assetUrlProvider;
    private readonly ICityMapEntityStyle _cityMapEntityStyle;
    private readonly HttpClient _httpClient;
    private readonly Lazy<Task> _initializeOnce;
    private readonly IStringLocalizer<FogResource> _loc;
    private readonly ILogger<ProductionRenderer> _logger;
    private readonly IMapTransformationComponent _mapTransformationComponent;
    private readonly SKSamplingOptions _sampling = new(SKFilterMode.Linear, SKMipmapMode.Linear);
    private readonly ITypefaceProvider _typefaceProvider;
    private SKImage? _atlasCache;
    private SKFont _currentNameFont = null!;
    private SKFont _defaultNameFont = null!;
    private SKImage? _greyscaleAtlasCache;
    private bool _isInitialized;
    private string _localizedHourAbbreviation = string.Empty;
    private string _localizedHoursAbbreviation = string.Empty;

    private string _localizedMinutesAbbreviation = string.Empty;
    private Dictionary<string, SKRect> _sprites = new();

    public ProductionRenderer(HttpClient httpClient,
        IAssetUrlProvider assetUrlProvider,
        IMapTransformationComponent mapTransformationComponent,
        ICityMapEntityStyle cityMapEntityStyle,
        ITypefaceProvider typefaceProvider,
        IStringLocalizer<FogResource> loc,
        ILogger<ProductionRenderer> logger)
    {
        _httpClient = httpClient;
        _assetUrlProvider = assetUrlProvider;
        _mapTransformationComponent = mapTransformationComponent;
        _cityMapEntityStyle = cityMapEntityStyle;
        _typefaceProvider = typefaceProvider;
        _loc = loc;
        _logger = logger;

        _initializeOnce = new Lazy<Task>(InitializeCoreAsync);
    }

    public Task InitializeAsync()
    {
        return _initializeOnce.Value;
    }

    public void UpdateFontSize()
    {
        var textSize = (int) MathF.Min(
            MathF.Round(_cityMapEntityStyle.ProductionTimeDefaultTextSize / _mapTransformationComponent.Scale),
            _cityMapEntityStyle.ProductionTimeDefaultTextSize);
        if ((int) _currentNameFont.Size != textSize)
        {
            _currentNameFont = new SKFont(_typefaceProvider.MainTypeface, textSize);
        }

        Console.Out.WriteLine(textSize);
    }

    public void Draw(SKCanvas canvas, SKRect bounds, string resourceId, int? productionTime, bool isUnchanged)
    {
        if (!_isInitialized)
        {
            return;
        }

        var icon = $"icon_{resourceId}";
        if (!_sprites.TryGetValue(icon, out var frame))
        {
            return;
        }

        var textWidth = 0f;
        var scale = Math.Min(ICON_SIZE / frame.Width, ICON_SIZE / frame.Height);
        var iconWidth = frame.Width * scale;
        var iconHeight = frame.Height * scale;
        var totalWidth = iconWidth;
        var labelPaint = !isUnchanged ? _cityMapEntityStyle.NameTextPaint : _cityMapEntityStyle.UnchangedNameTextPaint;
        var productionText = string.Empty;
        if (productionTime != null)
        {
            productionText = CreateProductionText(productionTime.Value);
            textWidth = _currentNameFont.MeasureText(productionText, labelPaint);
            totalWidth = iconWidth + GAP + textWidth;
        }

        var iconOffsetX = bounds.Left + (bounds.Width - totalWidth) / 2f;
        var offsetY = bounds.Top + (bounds.Height - iconHeight) / 2f;
        var labelOffsetX = iconOffsetX + iconWidth + GAP;

        var atlas = isUnchanged ? _greyscaleAtlasCache : _atlasCache;

        canvas.DrawImage(atlas, frame, new SKRect(iconOffsetX, offsetY, iconOffsetX + iconWidth, offsetY + iconHeight),
            _sampling);
        if (productionTime != null)
        {
            SkiaTextUtils.DrawText(canvas, productionText,
                new SKRect(labelOffsetX, offsetY, labelOffsetX + textWidth, offsetY + iconHeight), _currentNameFont,
                labelPaint);
        }
    }

    private string CreateProductionText(int productionTime)
    {
        return productionTime switch
        {
            < 3600 => $"{productionTime / 60}{_localizedMinutesAbbreviation}",
            3600 => $"1{_localizedHourAbbreviation}",
            _ => $"{productionTime / 3600}{_localizedHoursAbbreviation}",
        };
    }

    private async Task InitializeCoreAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        _localizedMinutesAbbreviation = _loc[FogResource.Common_Minutes_Abbr];
        _localizedHourAbbreviation = _loc[FogResource.Common_Hour_Abbr];
        _localizedHoursAbbreviation = _loc[FogResource.Common_Hours_Abbr];

        try
        {
            await LoadAtlasAsync();
            _greyscaleAtlasCache = CreateGrayscaleAtlas(_atlasCache!);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error setting up icon atlas");
            return;
        }

        await _typefaceProvider.InitializeAsync();

        _defaultNameFont = new SKFont(_typefaceProvider.MainTypeface,
            _cityMapEntityStyle.ProductionTimeDefaultTextSize);
        _currentNameFont = _defaultNameFont;

        _isInitialized = true;
    }

    private SKImage CreateGrayscaleAtlas(SKImage source)
    {
        var info = new SKImageInfo(source.Width, source.Height);
        using var surface = SKSurface.Create(info);
        using var canvas = surface.Canvas;

        // Luminance-based grayscale conversion
        var colorMatrix = new[]
        {
            0.21f, 0.72f, 0.07f, 0, GRAYSCALE_BRIGHTNESS,
            0.21f, 0.72f, 0.07f, 0, GRAYSCALE_BRIGHTNESS,
            0.21f, 0.72f, 0.07f, 0, GRAYSCALE_BRIGHTNESS,
            0, 0, 0, 1, 0,
        };

        using var paint = new SKPaint();
        paint.ColorFilter = SKColorFilter.CreateColorMatrix(colorMatrix);

        canvas.DrawImage(source, 0, 0, paint);

        var result = surface.Snapshot();

        return result;
    }

    private async Task LoadAtlasAsync()
    {
        var urls = _assetUrlProvider.GetHohIconAtlasUrl();
        await using var imageStream = await _httpClient.GetStreamAsync(urls.Image);
        _atlasCache = SKImage.FromEncodedData(imageStream);

        var atlasMeta = await _httpClient.GetFromJsonAsync<FreeTexturePackerAtlas>(urls.Meta);
        if (atlasMeta != null)
        {
            _sprites = atlasMeta.Sprites.ToDictionary(x => x.Key,
                x => new SKRect(x.Value.Frame.X, x.Value.Frame.Y, x.Value.Frame.W + x.Value.Frame.X,
                    x.Value.Frame.H + x.Value.Frame.Y));
        }
    }
}

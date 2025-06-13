using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class DefaultCityMapEntityStyle : ICityMapEntityStyle
{
    public IDictionary<BuildingType, SKPaint> BuildingTypePaints =
        new Dictionary<BuildingType, SKPaint>()
        {
            {
                BuildingType.CultureSite, new SKPaint()
                {
                    Color = SKColor.Parse("#A697E8"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Home, new SKPaint()
                {
                    Color = SKColor.Parse("#95BAE8"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Farm, new SKPaint()
                {
                    Color = SKColor.Parse("#CFE895"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Workshop, new SKPaint()
                {
                    Color = SKColor.Parse("#E8DF95"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Barracks, new SKPaint()
                {
                    Color = SKColor.Parse("#E8B995"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.RitualSite, new SKPaint()
                {
                    Color = SKColor.Parse("#A697E8"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Quarry, new SKPaint()
                {
                    Color = SKColor.Parse("#E8B995"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
            {
                BuildingType.Aviary, new SKPaint()
                {
                    Color = SKColor.Parse("#CFE895"),
                    IsAntialias = false,
                    Style = SKPaintStyle.Fill,
                }
            },
        };

    public SKPaint CultureFillPaint { get; } = new()
    {
        Color = SKColor.Parse("#A697E8"),
        IsAntialias = true,
        Style = SKPaintStyle.Fill,
    };

    public int NameDefaultTextSize { get; } = 16;

    public SKPaint NegativeSelectionFillPaint { get; } = new()
    {
        Color = SKColors.OrangeRed,
        IsAntialias = false,
        Style = SKPaintStyle.Fill,
    };

    public SKPaint PositiveSelectionFillPaint { get; } = new()
    {
        Color = SKColors.Green,
        IsAntialias = false,
        Style = SKPaintStyle.Fill,
    };

    public SKPaint CustomizationFillPaint { get; } = new SKPaint()
    {
        Color = SKColor.Parse("#E895CE"),
        IsAntialias = true,
        Style = SKPaintStyle.Fill,
    };

    public SKPaint NegativeSelectionStrokePaint { get; } = new()
    {
        Color = SKColors.DarkRed,
        StrokeWidth = 2,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
    };

    public SKPaint PositiveSelectionStrokePaint { get; } = new()
    {
        Color = SKColors.DarkGreen,
        StrokeWidth = 2,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
    };

    public SKPaint DefaultFillPaint { get; } = new()
    {
        Color = SKColor.Parse("#9B97A8"),
        IsAntialias = false,
        Style = SKPaintStyle.Fill,
    };

    public SKPaint DefaultStrokePaint { get; } = new()
    {
        Color = SKColor.Parse("#686945"),
        StrokeWidth = 0.5f,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
    };

    public SKPaint OverflowPaint { get; } = new()
    {
        Color = SKColor.Parse("#24000000"),
        IsAntialias = false,
        Style = SKPaintStyle.Fill,
    };

    public SKPaint OverflowStrokePaint { get; } = new()
    {
        Color = SKColor.Parse("#c5c5c5"),
        StrokeWidth = 2,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        PathEffect = SKPathEffect.CreateDash([5, 8], 0),
    };

    public SKPaint NameTextPaint { get; } = new()
    {
        Color = SKColors.Black,
        IsAntialias = true,
    };

    public SKPaint GetPaint(BuildingType buildingType)
    {
        if (BuildingTypePaints.TryGetValue(buildingType, out var paint))
        {
            return paint;
        }

        return DefaultFillPaint;
    }
}

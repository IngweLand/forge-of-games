using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class MapStyle
{
    public SKColor BackgroundColor { get; } = SKColor.Parse("#f0f0f0");

    public SKPaint ExpansionIconPaint { get; } = new()
    {
        Color = SKColor.Parse("#e0e0e0"),
        IsAntialias = true,
        Style = SKPaintStyle.StrokeAndFill
    };

    public IDictionary<ExpansionType, SKPaint> ExpansionPaints { get; } = new Dictionary<ExpansionType, SKPaint>()
    {
        {
            ExpansionType.Undefined, new SKPaint()
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 0.5f
            }
        },
        {
            ExpansionType.Blocker, new SKPaint()
            {
                Color = SKColor.Parse("#d0d0d0"),
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 0.5f
            }
        },
        {
            ExpansionType.Connector, new SKPaint()
            {
                Color = SKColor.Parse("#9D825C"),
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 0.5f
            }
        },
        {
            ExpansionType.Linked, new SKPaint()
            {
                Color = SKColor.Parse("#7EAED9"),
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 0.5f
            }
        },
        {
            ExpansionType.DetachedConnector, new SKPaint()
            {
                Color = SKColor.Parse("#9D825C"),
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 0.5f
            }
        }
    };

    public IDictionary<ExpansionSubType, SKPaint> ExpansionSubPaints { get; } =
        new Dictionary<ExpansionSubType, SKPaint>()
        {
            {
                ExpansionSubType.Water, new SKPaint()
                {
                    Color = SKColor.Parse("#efffff"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f
                }
            }
        };

    public SKPaint GridLinePaint { get; } = new()
    {
        Color = SKColor.Parse("#f0f0f0"),
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 1
    };

    public SKPaint HappinessPaint { get; } = new()
    {
        Color = SKColor.Parse("#40A697E8"),
        IsAntialias = false,
        Style = SKPaintStyle.Fill
    };

    public SKPaint LockedExpansionPaint { get; } = new()
    {
        Color = SKColor.Parse("#f5f5f5"),
        IsAntialias = true,
        Style = SKPaintStyle.Fill
    };
}
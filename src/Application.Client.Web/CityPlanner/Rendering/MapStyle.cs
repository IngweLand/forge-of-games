using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class MapStyle
{
    public SKColor BackgroundColor = SKColor.Parse("#f0f0f0");

    public SKPaint GridLinePaint = new()
    {
        Color = SKColor.Parse("#f0f0f0"),
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 1,
    };

    public IDictionary<ExpansionSubType, SKPaint> ExpansionSubPaints =
        new Dictionary<ExpansionSubType, SKPaint>()
        {
            {
                ExpansionSubType.Water, new SKPaint()
                {
                    Color = SKColor.Parse("#efffff"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
        };

    public SKPaint LockedExpansionPaint = new SKPaint()
    {
        Color = SKColors.Brown,
        IsAntialias = true,
        Style = SKPaintStyle.Fill,
    };

    public IDictionary<ExpansionType, SKPaint> ExpansionPaints =
        new Dictionary<ExpansionType, SKPaint>()
        {
            {
                ExpansionType.Undefined, new SKPaint()
                {
                    Color = SKColors.White,
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
            {
                ExpansionType.Blocker, new SKPaint()
                {
                    Color = SKColor.Parse("#d0d0d0"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
            {
                ExpansionType.Connector, new SKPaint()
                {
                    Color = SKColor.Parse("#9D825C"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
            {
                ExpansionType.Linked, new SKPaint()
                {
                    Color = SKColor.Parse("#7EAED9"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
            {
                ExpansionType.DetachedConnector, new SKPaint()
                {
                    Color = SKColor.Parse("#9D825C"),
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    StrokeWidth = 0.5f,
                }
            },
        };
}
